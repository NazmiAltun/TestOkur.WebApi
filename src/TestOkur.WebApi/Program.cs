namespace TestOkur.WebApi
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using App.Metrics;
	using App.Metrics.AspNetCore;
	using App.Metrics.Formatters.Prometheus;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Logging;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.WebApi.Data;

	public static class Program
	{
		private static IMetricsRoot Metrics { get; set; }

		public static async Task Main(string[] args)
		{
			Metrics = AppMetrics.CreateDefaultBuilder()
				.OutputMetrics.AsPrometheusPlainText()
				.OutputMetrics.AsPrometheusProtobuf()
				.Build();
			var host = BuildWebHost(args);
			await host.MigrateDbContextAsync<ApplicationDbContext>(async (context, services) =>
			{
				await DbInitializer.SeedAsync(context, services);
			});

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseMetrics(
					options =>
					{
						options.EndpointOptions = endpointsOptions =>
						{
							endpointsOptions.MetricsTextEndpointOutputFormatter = Metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
							endpointsOptions.MetricsEndpointOutputFormatter = Metrics.OutputMetricsFormatters.OfType<MetricsPrometheusProtobufOutputFormatter>().First();
						};
					})
				.UseMetricsWebTracking()
				.UseSentry(options =>
				{
					options.Release = "qa";
					options.MaxBreadcrumbs = 200;
					options.HttpProxy = null;
					options.DecompressionMethods = DecompressionMethods.None;
					options.MaxQueueItems = 100;
					options.ShutdownTimeout = TimeSpan.FromSeconds(5);
				})
				.ConfigureLogging((hostingContext, logging) =>
				 {
					 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
						 .AddConsole()
						 .AddDebug();
				 }).Build();
	}
}
