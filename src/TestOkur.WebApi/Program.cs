namespace TestOkur.WebApi
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Prometheus.DotNetRuntime;
    using System;
    using System.Threading.Tasks;
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
                    webBuilder.UseStartup<Startup>();
                });
    }
}
