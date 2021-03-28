namespace TestOkur.Notification
{
    using GreenPipes;
    using Hangfire;
    using Hangfire.Mongo;
    using Hangfire.Mongo.Migration.Strategies;
    using Hangfire.Mongo.Migration.Strategies.Backup;
    using HealthChecks.UI.Client;
    using IdentityModel;
    using MassTransit;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using Polly;
    using Polly.Extensions.Http;
    using Prometheus;
    using RazorLight;
    using SpanJson.AspNetCore.Formatter;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Consumers;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.HostedServices;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;
    using TestOkur.Notification.ScheduledTasks.DailyReport;
    using TestOkur.Notification.ScheduledTasks.LicenseExpirationNotice;
    using TestOkur.Notification.ScheduledTasks.ReEvaluateAllExams;
    using TestOkur.Notification.ScheduledTasks.SmsResender;
    using TestOkur.Serialization;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
            configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
            configuration.GetSection("ApplicationConfiguration").Bind(ApplicationConfiguration);
            configuration.GetSection("HangfireConfiguration").Bind(HangfireConfiguration);
        }

        private ApplicationConfiguration ApplicationConfiguration { get; } = new ApplicationConfiguration();

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        private HangfireConfiguration HangfireConfiguration { get; } = new HangfireConfiguration();

        private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyCorsPolicy", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            RegisterMappings();
            AddOptions(services);
            AddHealthCheck(services);
            services.AddSingleton<IEmailClient, EmailClient>();
            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<INotificationFacade, NotificationFacade>();
            services.AddScoped<ILicenseExpirationNoticeTask, LicenseExpirationNoticeTask>();
            services.AddScoped<IDailyReportTask, DailyReportTask>();
            services.AddScoped<IReEvaluateAllExamsTask, ReEvaluateAllExamsTask>();
            services.AddScoped<ISmsResender, SmsResender>();
            AddHttpClients(services);
            AddMessageBus(services);
            AddHostedServices(services);
            AddHangfire(services);
            AddAuthentication(services);
            AddPolicies(services);
            services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build());
            services.AddTransient<ISmsRepository, SmsRepository>();
            services.AddTransient<ISmsLogRepository, SmsLogRepository>();
            services.AddTransient<IEMailRepository, EMailRepository>();
            services.AddTransient<IStatsRepository, StatsRepository>();
            services.AddControllersWithViews()
                .AddSpanJsonCustom<ApiResolver<byte>>();
            services.AddHttpContextAccessor();
            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAnyCorsPolicy");

            app.UseHttpMetrics();
            UseHangfire(app);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics("/metrics-core");
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
                endpoints.MapDefaultControllerRoute();
            });
        }

        protected virtual void AddHostedServices(IServiceCollection services)
        {
            services.AddHostedService<BusService>();
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

        private void UseHangfire(IApplicationBuilder app)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1,
            });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicDashboardAuthorizationFilter(HangfireConfiguration),
                },
            });
            RecurringJob.AddOrUpdate<ILicenseExpirationNoticeTask>(
                notice => notice.NotifyUsersAsync(), Cron.Daily(17, 00));
            RecurringJob.AddOrUpdate<IDailyReportTask>(
                x => x.SendAsync(), Cron.Daily(20, 30));
            RecurringJob.AddOrUpdate<ISmsResender>(
                x => x.TryResendAsync(), Cron.Hourly);
            RecurringJob.AddOrUpdate<IReEvaluateAllExamsTask>(
                x => x.SendRequestAsync(), Cron.Never);
        }

        private void AddHangfire(IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                var mongoUrlBuilder = new MongoUrlBuilder($"{ApplicationConfiguration.ConnectionString}/{ApplicationConfiguration.Database}_Hangfire?authSource=admin");
                var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                var storageOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new NoneMongoBackupStrategy(),
                    },
                    CheckConnection = false,
                };
                config.UseColouredConsoleLogProvider(Hangfire.Logging.LogLevel.Debug);
                config.UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, storageOptions);
            });
        }

        private void RegisterMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Sms)))
            {
                BsonClassMap.RegisterClassMap<Sms>();
                BsonClassMap.RegisterClassMap<EMail>();
            }
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureAndValidate<SmtpConfiguration>(_configuration);
            services.ConfigureAndValidate<ApplicationConfiguration>(_configuration);
            services.ConfigureAndValidate<SmsConfiguration>(_configuration);
            services.ConfigureAndValidate<OAuthConfiguration>(_configuration);
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
                .AddRabbitMQ(rabbitMqUri, null, "rabbitmq")
                .AddIdentityServer(new Uri(OAuthConfiguration.Authority))
                .AddUrlGroup(new Uri(_configuration.GetValue<string>("WebApiUrl") + "hc"), "WebApi")
                .AddUrlGroup(new Uri(_configuration.GetValue<string>("ApplicationConfiguration:SeqUrl")), "Seq")
                .AddMongoDb(
                    ApplicationConfiguration.ConnectionString,
                    "config",
                    "mongo",
                    null);
        }

        private void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Private,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(c => c.Type == JwtClaimTypes.ClientId &&
                                                   c.Value == Clients.Private)));
                options.AddPolicy(
                    AuthorizationPolicies.Customer,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.IsInRole(Roles.Customer)));

                options.AddPolicy(
                    AuthorizationPolicies.Admin,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin)));
            });
        }

        private void AddMessageBus(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(GetConsumerTypes());
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var uriStr = $"rabbitmq://{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
                    cfg.Host(new Uri(uriStr), hc =>
                    {
                        hc.Username(RabbitMqConfiguration.Username);
                        hc.Password(RabbitMqConfiguration.Password);
                    });
                    cfg.ReceiveEndpoint("notification-queue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2000, 1000));

                        if (!_environment.IsDevelopment())
                        {
                            e.Consumer<DefaultFaultConsumer>(context.Container);
                        }

                        e.Consumer<NewUserRegisteredConsumer>(context.Container);
                        e.Consumer<SendSmsRequestReceivedConsumer>(context.Container);
                        e.Consumer<SendSmsRequestFailedConsumer>(context.Container);
                        e.Consumer<UserActivatedConsumer>(context.Container);
                        e.Consumer<UserSubscriptionExtendedConsumer>(context.Container);
                        e.Consumer<SmsCreditAddedConsumer>(context.Container);
                        e.Consumer<ResetPasswordTokenGeneratedConsumer>(context.Container);
                        e.Consumer<UserErrorReceivedConsumer>(context.Container);
                        e.Consumer<ReferredUserActivatedConsumer>(context.Container);
                    });
                    cfg.SetLoggerFactory(context.Container.GetRequiredService<ILoggerFactory>());
                }));
            });
        }

        private void AddHttpClients(IServiceCollection services)
        {
            services.AddTransient<SmsServiceLoggingHandler>();
            services.AddHttpClient<IOAuthClient, OAuthClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("OAuthConfiguration:Authority"));
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IWebApiClient, WebApiClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("WebApiUrl"));
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IReportClient, ReportClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("ReportUrl"));
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<ISmsClient, SmsClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration.GetValue<string>("SmsConfiguration:ServiceUrl"));
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddHttpMessageHandler<SmsServiceLoggingHandler>()
            .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(60, retryAttempt => TimeSpan.FromSeconds((retryAttempt * 30) + 1));
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(20, TimeSpan.FromSeconds(30));
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
