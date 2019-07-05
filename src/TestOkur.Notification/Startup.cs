namespace TestOkur.Notification
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Net.Http;
	using GreenPipes;
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
	using RazorLight;
	using TestOkur.Common.Configuration;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.Notification.Configuration;
	using TestOkur.Notification.Consumers;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Infrastructure.Clients;
	using TestOkur.Notification.Models;

	[ExcludeFromCodeCoverage]
	public class Startup : IStartup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
			Configuration.GetSection("ApplicationConfiguration").Bind(ApplicationConfiguration);
		}

		private IConfiguration Configuration { get; }

		private ApplicationConfiguration ApplicationConfiguration { get; } = new ApplicationConfiguration();

		private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

		public virtual IServiceProvider ConfigureServices(IServiceCollection services)
		{
			RegisterMappings();
			AddOptions(services);
			AddHealthCheck(services);
			services.AddSingleton<IEmailClient, EmailClient>();
			services.AddSingleton<ITemplateService, TemplateService>();
			services.AddSingleton<NotificationManager>();
			AddHttpClients(services);
			AddMessageBus(services);
			AddHostedServices(services);
			services.AddSingleton<IRazorLightEngine>(new RazorLightEngineBuilder()
				.UseMemoryCachingProvider()
				.Build());
			services.AddTransient<ISmsRepository, SmsRepository>();

			return services.BuildServiceProvider();
		}

		public void Configure(IApplicationBuilder app)
		{
			var hcOptions = new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
			};
			app.UseHealthChecks("/hc", hcOptions);
		}

		protected virtual void AddHostedServices(IServiceCollection services)
		{
			services.AddHostedService<BusService>();
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
				.AddRabbitMQ(rabbitMqUri);
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
					e.Consumer<AccountExtendedConsumer>(provider);
					e.Consumer<ResetPasswordTokenGeneratedConsumer>(provider);
				});
				cfg.UseExtensionsLogging(provider.GetRequiredService<ILoggerFactory>());
			}));
		}

		private void AddMassTransit(IServiceCollection services)
		{
			services.AddMassTransit(x =>
			{
				x.AddConsumer<NewUserRegisteredConsumer>();
				x.AddConsumer<SendSmsRequestReceivedConsumer>();
				x.AddConsumer<SendSmsRequestFailedConsumer>();
				x.AddConsumer<UserActivatedConsumer>();
				x.AddConsumer<AccountExtendedConsumer>();
				x.AddConsumer<ResetPasswordTokenGeneratedConsumer>();
			});
		}

		private void AddHttpClients(IServiceCollection services)
		{
			services.AddTransient<SmsServiceLoggingHandler>();
			services.AddHttpClient<OAuthClient>();
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
	}
}
