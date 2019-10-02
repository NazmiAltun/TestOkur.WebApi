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
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Paramore.Brighter.Extensions.DependencyInjection;
    using Paramore.Darker.AspNetCore;
    using Paramore.Darker.QueryLogging;
    using Polly;
    using Polly.Extensions.Http;
    using Prometheus;
    using StackExchange.Redis;
    using Swashbuckle.AspNetCore.Swagger;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Data;
    using TestOkur.Domain.Model.SmsModel;
    using TestOkur.Infrastructure;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.Infrastructure.Extensions;
    using TestOkur.Infrastructure.Monitoring;
    using TestOkur.Infrastructure.Mvc;
    using TestOkur.Infrastructure.Threading;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.City;
    using TestOkur.WebApi.Application.User;
    using TestOkur.WebApi.Application.User.Services;
    using TestOkur.WebApi.Configuration;
    using TestOkur.WebApi.Extensions;
    using TestOkur.WebApi.Infrastructure;
    using ConfigurationBuilder = CacheManager.Core.ConfigurationBuilder;

    [ExcludeFromCodeCoverage]
    public class Startup : IStartup
    {
        private const string CorsPolicyName = "EnableCorsToAll";

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
            Configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
        }

        public IHostingEnvironment Environment { get; }

        private IConfiguration Configuration { get; }

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            AddOptions(services);

            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<ClientErrorFilter>();
            services.AddMvc(options =>
                {
                    options.Filters.Add(new ProducesAttribute("application/json"));
                    options.Filters.Add(new ValidateInputFilter());
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            AddCqrsFramework(services);
            AddHealthChecks(services);
            AddSwagger(services);
            AddCache(services);
            AddDatabase(services);
            AddAuthentication(services);
            AddPolicies(services);
            AddMessageBus(services);
            AddHttpClients(services);
            RegisterServices(services);
            AddHostedServices(services);
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();

            app.UseHttpMetrics();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMetricServer("/metrics-core");
            UseHealthChecks(app);
            app.UseCors(CorsPolicyName);
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
            app.UseStaticFiles();
            InitializeFluentMappings();
            UseSwagger(app);
        }

        protected virtual void AddMessageBus(
            IServiceCollection services,
            Action<IRabbitMqReceiveEndpointConfigurator> configure = null)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(
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
                });

            services.AddSingleton<IPublishEndpoint>(busControl);
            services.AddSingleton<IBus>(busControl);
            services.AddSingleton(busControl);
            services.AddMassTransit();
        }

        protected virtual void AddDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Postgres");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableSensitiveDataLogging();
            });
            services.AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory>();
        }

        protected virtual void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = OAuthConfiguration.Authority;
                    options.RequireHttpsMetadata = OAuthConfiguration.RequireHttpsMetadata;
                    options.ApiName = OAuthConfiguration.ApiName;
                    options.JwtValidationClockSkew = TimeSpan.FromHours(24);
                });
        }

        private static void UseHealthChecks(IApplicationBuilder app)
        {
            var hcOptions = new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            };
            app.UseHealthChecks("/hc", hcOptions);
        }

        private void UseSwagger(IApplicationBuilder app)
        {
            if (!Configuration.GetValue<bool>("SwaggerEnabled"))
            {
                return;
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestOkur Web Api"); });
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
            services.AddSingleton<ICaptchaService, CaptchaService>();
            services.AddSingleton<ISmsCreditCalculator, SmsCreditCalculator>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();
            services.AddScoped<IProcessor, Processor>();
            services.AddScoped<ICommandQueryLogger, CommandQueryLogger>();
            services.AddHttpContextAccessor();
        }

        private void AddCqrsFramework(IServiceCollection services)
        {
            services.AddDarker()
                .AddHandlersFromAssemblies(typeof(GetAllCitiesQuery).Assembly)
                .AddJsonQueryLogging()
                .AddCustomDecorators();

            services.AddBrighter()
                .AsyncHandlersFromAssemblies(Assembly.GetExecutingAssembly())
                .AddPipelineHandlers();
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "TestOkur Web Api", Version = "v1" });
            });
        }

        private void AddHealthChecks(IServiceCollection services)
        {
            var rabbitMqUri = $@"amqp://{RabbitMqConfiguration.Username}:{RabbitMqConfiguration.Password}@{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
            services.AddHealthChecks()
                .AddNpgSql(Configuration.GetConnectionString("Postgres"))
                .AddRedis(Configuration.GetConnectionString("Redis"))
                .AddIdentityServer(new Uri(OAuthConfiguration.Authority))
                .AddRabbitMQ(rabbitMqUri);
        }

        private void AddCache(IServiceCollection services)
        {
            var cacheManagerConfig =
                ConfigurationBuilder.BuildConfiguration(cfg =>
                {
                    cfg.WithJsonSerializer()
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

        private void AddHostedServices(IServiceCollection services)
        {
            if (!Environment.IsDevelopment())
            {
                services.AddHostedService<OnlineUserTrackerCleanupService>();
            }
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
