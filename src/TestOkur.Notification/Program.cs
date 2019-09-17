using System;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus.DotNetRuntime;

[assembly: InternalsVisibleTo("TestOkur.Notification.Unit.Tests")]

namespace TestOkur.Notification
{
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
