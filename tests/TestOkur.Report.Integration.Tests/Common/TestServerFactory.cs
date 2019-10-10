namespace TestOkur.Report.Integration.Tests.Common
{
    using IdentityModel;
    using MassTransit.RabbitMqTransport;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Security.Claims;
    using TestOkur.Common;
    using TestOkur.TestHelper;

    public class TestServerFactory : TestServerFactory<Startup>
    {
        static TestServerFactory()
        {
            TestServer = new TestServerFactory()
                .Create();
        }

        public static TestServer TestServer { get; }

        public override TestServer Create(Action<IServiceCollection> configureServices = null)
        {
            return base.Create(services =>
            {
                configureServices?.Invoke(services);
                DefaultConfigureServices(services);
            });
        }

        private static void DefaultConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Action<IRabbitMqReceiveEndpointConfigurator>>(cfg => { Consumer.Instance.Configure(cfg); });
            var subjectClaim = services.BuildServiceProvider()
                .GetService<Claim>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test Scheme";
                    options.DefaultChallengeScheme = "Test Scheme";
                })
                .AddTestAuth<TestAuthenticationOptions>(o =>
                {
                    o.Identity = new ClaimsIdentity(
                        new[]
                    {
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.ClientId, Clients.Public),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Admin),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.Customer),
                        subjectClaim ?? new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                    }, "test");
                });
        }
    }
}
