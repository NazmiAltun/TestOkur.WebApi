namespace TestOkur.Report
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
                .ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
					.AddConsole()
					.AddDebug();
				}).Build();
	}
}
