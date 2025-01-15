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
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public static class Program
    {
        public static void Main(string[] args)
        {
            DotNetRuntimeStatsBuilder.Default().WithErrorHandler(e =>
            {
                Console.WriteLine(e.ToString());
            }).StartCollecting();
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            var loggerConfig = loggerConfiguration
                                .ReadFrom.Configuration(hostingContext.Configuration)
                                .MinimumLevel.Warning()
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly().GetName().Name)
                                .Filter.ByExcluding(x => x.Exception is ValidationException)
                                .WriteTo.Console();

                            if (!hostingContext.HostingEnvironment.IsProduction())
                            {
                                loggerConfig.WriteTo.Console();
                            }
                        })
                        .UseStartup<Startup>();
                });
    }
}
