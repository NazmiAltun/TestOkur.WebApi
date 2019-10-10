namespace TestOkur.WebApi.Integration.Tests.Common
{
    using IdentityModel;
    using MassTransit.RabbitMqTransport;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Extensions;
    using TestOkur.TestHelper;
    using TestOkur.WebApi.Data;

    public class TestServerFactory : TestServerFactory<Startup>
    {
        public async Task<TestServer> CreateAsync(Action<IServiceCollection> configureServices = null)
        {
            var testServer = Create(services =>
            {
                configureServices?.Invoke(services);
                DefaultConfigureServices(services);
            });
            await testServer.Host.MigrateDbContextAsync<ApplicationDbContext>(async (context, services) =>
            {
                await DbInitializer.SeedAsync(context, services);
            });
            return testServer;
        }

        private static void DefaultConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Action<IRabbitMqReceiveEndpointConfigurator>>(cfg =>
            {
                Consumer.Instance.Configure(cfg);
            });
            var sp = services.BuildServiceProvider();
            var subjectClaim = sp.GetService<Claim>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth<TestAuthenticationOptions>(o =>
            {
                o.Identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.ClientId,  Clients.Public),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Customer),
                        subjectClaim ?? new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                    }, "test");
            });
        }
    }
}
