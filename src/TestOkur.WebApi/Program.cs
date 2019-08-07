namespace TestOkur.WebApi
{
	using System;
	using System.Net;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Logging;
	using Prometheus;
	using Prometheus.DotNetRuntime;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.WebApi.Data;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			DotNetRuntimeStatsBuilder.Default().WithErrorHandler(e =>
			{
				Console.WriteLine(e.ToString());
			}).StartCollecting();
			var metricServer = new MetricServer(80, "metrics-core/");
			metricServer.Start();

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
