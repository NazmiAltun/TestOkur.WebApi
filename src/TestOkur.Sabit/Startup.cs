namespace TestOkur.Sabit
{
    using CacheManager.Core;
    using HealthChecks.UI.Client;
    using MassTransit;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Prometheus;
    using SpanJson.AspNetCore.Formatter;
    using SpanJson.Resolvers;
    using System;
    using System.Reflection;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Infrastructure.CommandsQueries.Extensions;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Sabit.Configuration;
    using TestOkur.Sabit.Infrastructure;
    using ConfigurationBuilder = CacheManager.Core.ConfigurationBuilder;

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

        private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            AddHealthChecks(services);
            AddOptions(services);
            AddAuthentication(services);
            AddAuthorization(services);
            AddCache(services);
            AddMessageBus(services);
            services.AddSingleton<IUserIdProvider, StubUserIdProvider>();
            services.AddSingleton<ICommandQueryLogger, StubCommandQueryLogger>();
            services.AddControllers()
                .AddSpanJsonCustom<ExcludeNullsOriginalCaseResolver<byte>>();
            services.AddQueries(Assembly.GetExecutingAssembly());
            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCompression();
            app.UseCors(CorsPolicyName);
            app.UseHttpMetrics();
            app.UseMetricServer("/metrics-core");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureAndValidate<ApplicationConfiguration>(Configuration);
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApplicationConfiguration>>().Value);
        }

        private void AddHealthChecks(IServiceCollection services)
        {
            var rabbitMqUri = $@"amqp://{RabbitMqConfiguration.Username}:{RabbitMqConfiguration.Password}@{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
            services.AddHealthChecks()
                .AddRabbitMQ(rabbitMqUri, null, "rabbitmq")
                .AddIdentityServer(new Uri(OAuthConfiguration.Authority));
        }

        private void AddCache(IServiceCollection services)
        {
            var cacheManagerConfig =
                ConfigurationBuilder.BuildConfiguration(cfg =>
                {
                    cfg.WithGzJsonSerializer()
                        .WithMicrosoftMemoryCacheHandle("runTimeMemory");
                });

            services.AddSingleton(cacheManagerConfig);
            services.AddCacheManager();
        }

        private void AddAuthentication(IServiceCollection services)
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

        private void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Public,
                    policy => policy.RequireAssertion(context =>
                        context.User.Identity.IsAuthenticated));
            });
        }

        private void AddMessageBus(IServiceCollection services)
        {
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
                        services.AddSingleton(host);
                    }));
            });

            if (Environment.IsDevelopment())
            {
                services.BuildServiceProvider()
                    .GetService<IBusControl>().Start();
            }
        }
    }
}
