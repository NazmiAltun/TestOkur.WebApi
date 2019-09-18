namespace TestOkur.WebApi
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
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
                .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
                         .AddConsole()
                         .AddDebug();
                 }).Build();
    }
}
