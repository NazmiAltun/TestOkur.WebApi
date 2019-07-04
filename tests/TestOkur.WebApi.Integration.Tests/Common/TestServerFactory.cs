namespace TestOkur.WebApi.Integration.Tests.Common
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Extensions;
	using TestOkur.TestHelper;
	using TestOkur.WebApi.Data;

	public class TestServerFactory : TestServerFactory<TestStartup>
	{
		public async Task<TestServer> CreateAsync(Action<IServiceCollection> configureServices = null)
		{
			var testServer = Create(configureServices);
			await testServer.Host.MigrateDbContextAsync<ApplicationDbContext>(async (context, services) =>
			{
				await DbInitializer.SeedAsync(context);
			});
			return testServer;
		}
	}
}
