namespace TestOkur.Notification
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using GreenPipes;
    using Hangfire;
    using Hangfire.Mongo;
    using HealthChecks.UI.Client;
    using MassTransit;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson.Serialization;
    using Polly;
    using Polly.Extensions.Http;
    using Prometheus;
    using RazorLight;
    using TestOkur.Common.Configuration;
    using TestOkur.Infrastructure.Extensions;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Consumers;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;
    using TestOkur.Notification.ScheduledTasks;

    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
            Configuration.GetSection("ApplicationConfiguration").Bind(ApplicationConfiguration);
            Configuration.GetSection("HangfireConfiguration").Bind(HangfireConfiguration);
        }

        private IConfiguration Configuration { get; }

        private ApplicationConfiguration ApplicationConfiguration { get; } = new ApplicationConfiguration();

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private HangfireConfiguration HangfireConfiguration { get; } = new HangfireConfiguration();
        
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterMappings();
            AddOptions(services);
            AddHealthCheck(services);
            services.AddSingleton<IEmailClient, EmailClient>();
            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<INotificationFacade, NotificationFacade>();
            services.AddScoped<ISendLicenseExpirationNotice, SendLicenseExpirationNotice>();
            services.AddScoped<IDailyReport, DailyReport>();

            AddHttpClients(services);
            AddMessageBus(services);
            AddHostedServices(services);
            AddHangfire(services);
            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build());
            services.AddTransient<ISmsRepository, SmsRepository>();
            services.AddMvc();
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            UseHangfire(app);
            app.UseHttpMetrics();
            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
            app.UseMetricServer("/metrics-core");
            app.UseMvc();
            app.UseStaticFiles();
        }

        protected virtual void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<BusService>();
        }

        private void UseHangfire(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicDashboardAuthorizationFilter(HangfireConfiguration),
                },
            });
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1,
            });
            RecurringJob.AddOrUpdate<ISendLicenseExpirationNotice>(
                notice => notice.NotifyUsersAsync(), Cron.Daily(17, 00));
            RecurringJob.AddOrUpdate<IDailyReport>(
                x => x.SendAsync(), Cron.Daily(20, 30));
        }

        private void AddHangfire(IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config.UseColouredConsoleLogProvider(Hangfire.Logging.LogLevel.Debug);
                var migrationOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        Strategy = MongoMigrationStrategy.Migrate,
                        BackupStrategy = MongoBackupStrategy.None,
                    },
                };
                config.UseMongoStorage(
                    ApplicationConfiguration.ConnectionString,
                    $"{ApplicationConfiguration.Database}-Hangfire",
                    migrationOptions);
            });
        }

        private void RegisterMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Sms)))
            {
                BsonClassMap.RegisterClassMap<Sms>();
            }
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureAndValidate<SmtpConfiguration>(Configuration);
            services.ConfigureAndValidate<ApplicationConfiguration>(Configuration);
            services.ConfigureAndValidate<SmsConfiguration>(Configuration);
            services.ConfigureAndValidate<OAuthConfiguration>(Configuration);
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<SmtpConfiguration>>().Value);
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<SmsConfiguration>>().Value);
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApplicationConfiguration>>().Value);
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<OAuthConfiguration>>().Value);
        }

        private void AddHealthCheck(IServiceCollection services)
        {
            var rabbitMqUri = $@"amqp://{RabbitMqConfiguration.Username}:{RabbitMqConfiguration.Password}@{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
            services.AddHealthChecks()
                .AddRabbitMQ(rabbitMqUri)
                .AddMongoDb(
                    ApplicationConfiguration.ConnectionString,
                    ApplicationConfiguration.Database,
                    "mongodb",
                    null);
        }

        private void AddMessageBus(IServiceCollection services)
        {
            AddMassTransit(services);
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var uriStr = $"rabbitmq://{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
                var host = cfg.Host(new Uri(uriStr), hc =>
                {
                    hc.Username(RabbitMqConfiguration.Username);
                    hc.Password(RabbitMqConfiguration.Password);
                });

                cfg.ReceiveEndpoint(host, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.Consumer<NewUserRegisteredConsumer>(provider);
                    e.Consumer<SendSmsRequestReceivedConsumer>(provider);
                    e.Consumer<SendSmsRequestFailedConsumer>(provider);
                    e.Consumer<UserActivatedConsumer>(provider);
                    e.Consumer<UserSubscriptionExtendedConsumer>(provider);
                    e.Consumer<SmsCreditAddedConsumer>(provider);
                    e.Consumer<ResetPasswordTokenGeneratedConsumer>(provider);
                    e.Consumer<UserErrorReceivedConsumer>(provider);
                });
                cfg.UseExtensionsLogging(provider.GetRequiredService<ILoggerFactory>());
            }));
        }

        private void AddMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetConsumerTypes());
            });
        }

        private void AddHttpClients(IServiceCollection services)
        {
            services.AddTransient<SmsServiceLoggingHandler>();
            services.AddHttpClient<IOAuthClient, OAuthClient>(client =>
                {
                    client.BaseAddress = new Uri(Configuration.GetValue<string>("OAuthConfiguration:Authority"));
                }).AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IWebApiClient, WebApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("WebApiUrl"));
            }).AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<ISmsClient, SmsClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("SmsConfiguration:ServiceUrl"));
            }).AddPolicyHandler(GetRetryPolicy())
            .AddHttpMessageHandler<SmsServiceLoggingHandler>()
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

        private Type[] GetConsumerTypes()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                            typeof(IConsumer).IsAssignableFrom(t))
                .ToArray();
        }
    }
}
