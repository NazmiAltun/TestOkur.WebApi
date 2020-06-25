namespace TestOkur.WebApi
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Prometheus.DotNetRuntime;
    using Serilog;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.WebApi.Data;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            DotNetRuntimeStatsBuilder.Default().WithErrorHandler(e =>
            {
                Console.WriteLine(e.ToString());
            }).StartCollecting();

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var host = CreateHostBuilder(args).Build();
            await host.MigrateDbContextAsync<ApplicationDbContext>(async (context, services) =>
            {
                await DbInitializer.SeedAsync(context, services);
            });

            host.Run();
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
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplicationName", Assembly.GetEntryAssembly().GetName().Name)
                                .MinimumLevel.Warning()
                                .Filter.ByExcluding(x => x.Exception is ValidationException)
                                .WriteTo.Seq(
                                    hostingContext.Configuration.GetValue<string>("ApplicationConfiguration:SeqUrl"));

                            if (!hostingContext.HostingEnvironment.IsProduction())
                            {
                                loggerConfig.WriteTo.Console();
                            }
                        })
                        .UseStartup<Startup>();
                });
    }
}
