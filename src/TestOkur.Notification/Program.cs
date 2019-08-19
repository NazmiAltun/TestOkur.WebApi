using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Notification.Unit.Tests")]

namespace TestOkur.Notification
{
	using System;
	using System.Net;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Logging;
	using Prometheus.DotNetRuntime;

	public static class Program
	{
		public static void Main(string[] args)
		{
			DotNetRuntimeStatsBuilder.Default().WithErrorHandler(e =>
			{
				Console.WriteLine(e.ToString());
			}).StartCollecting();
			BuildWebHost(args).Run();
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
