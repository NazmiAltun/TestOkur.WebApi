namespace TestOkur.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Prometheus.DotNetRuntime;
    using Serilog;

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
                        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .Filter.ByExcluding(x => x.Exception is ValidationException)
                            .Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly().GetName().Name)
                            .MinimumLevel.Warning()
                            .WriteTo.Console()
                            .WriteTo.Seq(hostingContext.Configuration.GetValue<string>("ReportConfiguration:SeqUrl")))
                        .UseStartup<Startup>();
                });
    }
}
