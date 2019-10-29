namespace TestOkur.Sabit
{
    using CacheManager.Core;
    using HealthChecks.UI.Client;
    using IdentityModel;
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Prometheus;
    using System;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using ConfigurationBuilder = CacheManager.Core.ConfigurationBuilder;

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
            Configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
        }

        private OAuthConfiguration OAuthConfiguration { get;  }

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            AddHealthChecks(services);
            AddAuthentication(services);
            AddAuthorization(services);
            AddCache(services);
            AddMessageBus(services);
            services.AddControllers();
            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCompression();
            app.UseCors();
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

        private void AddHealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks();
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

        private void AddAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Public,
                    policy => policy.RequireAssertion(context =>
                        context.User.Identity.IsAuthenticated ||
                        context.User.HasClaim(c => c.Type == JwtClaimTypes.ClientId &&
                                                   c.Value == Clients.Public)));
            });
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
    }
}
