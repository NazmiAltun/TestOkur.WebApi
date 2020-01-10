using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Notification.Unit.Tests")]

namespace TestOkur.Notification
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Prometheus.DotNetRuntime;
    using Serilog;
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            DotNetRuntimeStatsBuilder.Default().WithErrorHandler(e =>
            {
                Console.WriteLine(e.ToString());
            }).StartCollecting();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.Seq(hostingContext.Configuration.GetValue<string>("ApplicationConfiguration:SeqUrl"))
                            .WriteTo.Console())
                        .UseStartup<Startup>();
                });
    }
}
