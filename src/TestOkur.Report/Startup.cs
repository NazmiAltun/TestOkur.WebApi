using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using TestOkur.Domain.SeedWork;
using TestOkur.Report.Extensions;

[assembly: InternalsVisibleTo("TestOkur.Report.Integration.Tests")]

namespace TestOkur.Report
{
	using System;
	using GreenPipes;
	using HealthChecks.UI.Client;
	using MassTransit;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics.HealthChecks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization;
	using MongoDB.Bson.Serialization.IdGenerators;
	using MongoDB.Bson.Serialization.Serializers;
	using TestOkur.Common;
	using TestOkur.Common.Configuration;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.Infrastructure.Mvc;
	using TestOkur.Optic.Form;
	using TestOkur.Report.Configuration;
	using TestOkur.Report.Consumers;
	using TestOkur.Report.Infrastructure;
	using TestOkur.Report.Repositories;

	public class Startup : IStartup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			configuration.GetSection("OAuthConfiguration").Bind(OAuthConfiguration);
			configuration.GetSection("ReportConfiguration").Bind(ReportConfiguration);
			Configuration.GetSection("RabbitMqConfiguration").Bind(RabbitMqConfiguration);
		}

		public IConfiguration Configuration { get; }

		private ReportConfiguration ReportConfiguration { get; } = new ReportConfiguration();

		private OAuthConfiguration OAuthConfiguration { get; } = new OAuthConfiguration();

		private RabbitMqConfiguration RabbitMqConfiguration { get; } = new RabbitMqConfiguration();

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			RegisterMappings();
			AddHealthCheck(services);
			AddOptions(services);
			services.AddMvc(options =>
			{
				options.Filters.Add(new ProducesAttribute("application/json"));
				options.Filters.Add(new ValidateInputFilter());
			});
			AddAuthentication(services);
			AddPolicies(services);
			AddMessageBus(services);
			RegisterServices(services);
			AddHostedServices(services);

			return services.BuildServiceProvider();
		}

		public void Configure(IApplicationBuilder app)
		{
			var env = app.ApplicationServices.GetService<IHostingEnvironment>();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			var hcOptions = new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			};
			app.UseHealthChecks("/hc", hcOptions);
			app.UseAuthentication();
			app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<RequestResponseLoggingMiddleware>();
			app.UseMvc();
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
				});
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

				BsonClassMap.RegisterClassMap<StudentOpticalForm>();
				BsonClassMap.RegisterClassMap<AnswerKeyOpticalForm>();
			}
		}

		private void RegisterServices(IServiceCollection services)
		{
			services.AddTransient<IOpticalFormRepository, OpticalFormRepository>();
			services.AddSingleton<IRequestResponseLogger, RequestResponseMongodbLogger>();
			services.AddHttpContextAccessor();
		}

		private void AddPolicies(IServiceCollection services)
		{
			services.AddAuthorization(options => options.AddPolicy(
				AuthorizationPolicies.Customer,
				policy => policy.RequireAssertion(context =>
					context.User.IsInRole(Roles.Admin) ||
					context.User.IsInRole(Roles.Customer))));
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
					null,
					null);
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
					 e.RegisterConsumers(provider);
				 });
				 cfg.UseExtensionsLogging(new LoggerFactory());
			 }));
			services.AddSingleton<IPublishEndpoint>(x => x.GetService<IBusControl>());
		}

		private void AddMassTransit(IServiceCollection services)
		{
			services.AddMassTransit(x =>
			{
				x.AddConsumers(GetConsumerTypes());
			});
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
