using TestOkur.Infrastructure.CommandsQueries.Extensions;

namespace TestOkur.WebApi
{
    using CacheManager.Core;
    using Dapper;
    using Dapper.FluentMap;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using IdentityModel;
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Paramore.Brighter.Extensions.DependencyInjection;
    using Paramore.Darker.AspNetCore;
    using Polly;
    using Polly.Extensions.Http;
    using Prometheus;
    using StackExchange.Redis;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Data;
    using TestOkur.Domain.Model.SmsModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Infrastructure.Mvc.Monitoring;
    using TestOkur.Infrastructure.Mvc.Mvc;
    using TestOkur.Infrastructure.Mvc.Threading;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.City;
    using TestOkur.WebApi.Application.User.Services;
    using TestOkur.WebApi.Configuration;
    using TestOkur.WebApi.Extensions;
    using TestOkur.WebApi.Infrastructure;
    using ConfigurationBuilder = CacheManager.Core.ConfigurationBuilder;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CorsPolicyName = "EnableCorsToAll";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
            Configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
        }

        public IWebHostEnvironment Environment { get; }

        private IConfiguration Configuration { get; }

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

        public void ConfigureServices(IServiceCollection services)
        {
            AddOptions(services);

            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMemoryCache();
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<ClientErrorFilter>();
            services.AddControllers(options =>
                {
                    options.Filters.Add(new ProducesAttribute("application/json"));
                    options.Filters.Add(new ValidateInputFilter());
                })
               .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               });

            services.AddCommandsAndQueries();
            AddHealthChecks(services);
            AddCache(services);
            AddDatabase(services);
            AddAuthentication(services);
            AddPolicies(services);
            AddMessageBus(services);
            AddHttpClients(services);
            RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseHttpMetrics();
            app.UseMetricServer("/metrics-core");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
                endpoints.MapDefaultControllerRoute();
            });
            InitializeFluentMappings();
        }

        private void AddMessageBus(IServiceCollection services)
        {
            var configure = services.BuildServiceProvider().GetService<Action<IRabbitMqReceiveEndpointConfigurator>>();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(
                    cfg =>
                    {
                        var uriStr = $"rabbitmq://{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
                        var host = cfg.Host(new Uri(uriStr), hc =>
                        {
                            hc.Username(RabbitMqConfiguration.Username);
                            hc.Password(RabbitMqConfiguration.Password);
                        });

                        if (configure != null)
                        {
                            cfg.ReceiveEndpoint(host, configure);
                        }
                    }));
            });

            if (Environment.IsDevelopment())
            {
                services.BuildServiceProvider()
                    .GetService<IBusControl>().Start();
            }
        }

        private void AddDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Postgres");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(
                    connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)).Options;
            services.AddSingleton(dbContextOptions);
            services.AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory>();
            services.AddDbContext<ApplicationDbContext>();
        }

        private void AddAuthentication(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                return;
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = OAuthConfiguration.Authority;
                    options.RequireHttpsMetadata = OAuthConfiguration.RequireHttpsMetadata;
                    options.ApiName = OAuthConfiguration.ApiName;
                    options.JwtValidationClockSkew = TimeSpan.FromHours(24);
                });
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureAndValidate<ApplicationConfiguration>(Configuration);
            services.ConfigureAndValidate<OAuthConfiguration>(Configuration);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApplicationConfiguration>>().Value);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<OAuthConfiguration>>().Value);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient<ICaptchaService, CaptchaService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("CaptchaServiceUrl"));
            });
            services.AddSingleton<ISmsCreditCalculator, SmsCreditCalculator>();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddSingleton<ICommandQueryLogger, CommandQueryLogger>();
            services.AddSingleton<ITelemetryInitializer, HeaderTelemetryInitializer>();
            services.AddHttpContextAccessor();
        }
        
        private void AddHealthChecks(IServiceCollection services)
        {
            var rabbitMqUri = $@"amqp://{RabbitMqConfiguration.Username}:{RabbitMqConfiguration.Password}@{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("Postgres"))
                .AddUrlGroup(new Uri(Configuration.GetValue<string>("CaptchaServiceUrl") + "hc"))
                .AddRedis(Configuration.GetConnectionString("Redis"))
                .AddIdentityServer(new Uri(OAuthConfiguration.Authority))
                .AddRabbitMQ(rabbitMqUri);
        }

        private void AddCache(IServiceCollection services)
        {
            var cacheManagerConfig =
                ConfigurationBuilder.BuildConfiguration(cfg =>
                {
                    cfg.WithGzJsonSerializer()
                        .WithRedisConfiguration("redis", Configuration.GetConnectionString("Redis"))
                        .WithRedisBackplane("redis")
                        .WithRedisCacheHandle("redis", true);
                });

            services.AddSingleton(cacheManagerConfig);
            services.AddCacheManager();

            var redisConnection = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("DataProtection-Keys"));
        }

        private void InitializeFluentMappings()
        {
            using (CrossProcessLockFactory.CreateCrossProcessLock())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (FluentMapper.EntityMaps.IsEmpty)
                {
                    FluentMapper.Initialize(config =>
                    {
                        config.AddMapFromCurrentAssembly();
                    });
                }
            }
        }

        private void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Public,
                    policy => policy.RequireAssertion(context =>
                        context.User.Identity.IsAuthenticated ||
                        context.User.HasClaim(c => c.Type == JwtClaimTypes.ClientId &&
                                                   c.Value == Clients.Public)));
                options.AddPolicy(
                    AuthorizationPolicies.Private,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(c => c.Type == JwtClaimTypes.ClientId &&
                                                   c.Value == Clients.Private)));
                options.AddPolicy(
                    AuthorizationPolicies.Customer,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin) || context.User.IsInRole(Roles.Customer)));

                options.AddPolicy(
                    AuthorizationPolicies.Admin,
                    policy => policy.RequireRole(Roles.Admin));
            });
        }

        private void AddHttpClients(IServiceCollection services)
        {
            services.AddHttpClient<IIdentityService, IdentityService>(client =>
            {
                client.BaseAddress = new Uri(OAuthConfiguration.Authority);
            }).AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
