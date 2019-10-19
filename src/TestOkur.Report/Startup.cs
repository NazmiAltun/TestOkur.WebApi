﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Report.Integration.Tests")]

namespace TestOkur.Report
{
    using CacheManager.Core;
    using GreenPipes;
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
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Bson.Serialization.Serializers;
    using Prometheus;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Infrastructure.Mvc.Monitoring;
    using TestOkur.Infrastructure.Mvc.Mvc;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Configuration;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Extensions;
    using TestOkur.Report.Infrastructure;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Report.Models;
    using ConfigurationBuilder = CacheManager.Core.ConfigurationBuilder;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CorsPolicyName = "EnableCorsToAll";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
            configuration.GetSection("ReportConfiguration").Bind(ReportConfiguration);
            Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        private ReportConfiguration ReportConfiguration { get; } = new ReportConfiguration();

        private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

        private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            RegisterMappings();
            AddHealthCheck(services);
            AddCache(services);
            AddOptions(services);
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<ClientErrorFilter>();
            services.AddControllers(options =>
                {
                    options.Filters.Add(new ProducesAttribute("application/json"));
                    options.Filters.Add(new ValidateInputFilter());
                }).AddNewtonsoftJson();
            AddAuthentication(services);
            AddPolicies(services);
            AddMessageBus(services);
            RegisterServices(services);
            AddHostedServices(services);
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
            app.UseMiddleware<ErrorHandlingMiddleware>();
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

        private void AddHostedServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                return;
            }

            services.AddHostedService<BusService>();
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

        private void AddMessageBus(IServiceCollection services)
        {
            var configure = services.BuildServiceProvider().GetService<Action<IRabbitMqReceiveEndpointConfigurator>>();
            services.AddMassTransit(m =>
            {
                m.AddConsumers(GetConsumerTypes());
                m.AddBus(provider =>
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
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

                        cfg.ReceiveEndpoint(host, e =>
                        {
                            e.PrefetchCount = 16;
                            e.UseMessageRetry(x => x.Interval(2, 100));
                            e.RegisterConsumers(provider, Environment.IsDevelopment());
                        });
                        cfg.UseExtensionsLogging(new LoggerFactory());
                    }));
            });

            if (Environment.IsDevelopment())
            {
                services.BuildServiceProvider()
                    .GetService<IBusControl>()
                    .Start();
            }
        }

        private void RegisterMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(OpticalForm)))
            {
                BsonClassMap.RegisterClassMap<OpticalForm>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(StringObjectIdGenerator.Instance)
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
                BsonClassMap.RegisterClassMap<ReportRequest>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<StudentOpticalForm>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<AnswerKeyOpticalForm>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<SchoolResult>(cm => { cm.AutoMap(); });
            }
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IStudentOpticalFormRepository, StudentOpticalFormRepository>();
            services.AddTransient<IAnswerKeyOpticalFormRepository, AnswerKeyOpticalFormRepository>();
            services.AddTransient<IReportRequestRepository, ReportRequestRepository>();
            services.AddTransient<ISchoolResultRepository, SchoolResultRepository>();
            services.AddSingleton<IRequestResponseLogger, RequestResponseMongodbLogger>();
            services.AddSingleton<IEvaluator, Evaluator>();
            services.AddHttpContextAccessor();
        }

        private void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.Admin,
                    policy => policy.RequireAssertion(context =>
                        context.User.IsInRole(Roles.Admin)));

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
            });
        }

        private void AddHealthCheck(IServiceCollection services)
        {
            var rabbitMqUri =
                $@"amqp://{RabbitMqConfiguration.Username}:{RabbitMqConfiguration.Password}@{RabbitMqConfiguration.Uri}/{RabbitMqConfiguration.Vhost}";
            services.AddHealthChecks()
                .AddRabbitMQ(rabbitMqUri)
                .AddIdentityServer(new Uri(OAuthConfiguration.Authority))
                .AddMongoDb(
                    ReportConfiguration.ConnectionString,
                    ReportConfiguration.Database,
                    "mongodb",
                    null);
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
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("DataProtection-Keys"));
        }

        private void AddOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureAndValidate<ReportConfiguration>(Configuration);
            services.ConfigureAndValidate<OAuthConfiguration>(Configuration);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ReportConfiguration>>().Value);

            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<OAuthConfiguration>>().Value);
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
