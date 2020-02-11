namespace TestOkur.Sabit
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
                            .WriteTo.Console()
                            .Filter.ByExcluding(x => x.Exception is ValidationException)
                            .Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly().GetName().Name)
                            .WriteTo.Seq(hostingContext.Configuration.GetValue<string>("ApplicationConfiguration:SeqUrl"))
                            .MinimumLevel.Warning())
                        .UseStartup<Startup>();
                });
    }
}
