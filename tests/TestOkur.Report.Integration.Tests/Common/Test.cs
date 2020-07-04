namespace TestOkur.Report.Integration.Tests.Common
{
    using IdentityModel;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System.Security.Claims;

    public abstract class Test
    {
        private static readonly TestServerFactory TestServerFactory = new TestServerFactory();
        private TestServer _testServer;

        public TestServer Create(int userId)
        {
            if (_testServer == null)
            {
                void Configure(IServiceCollection services)
                {
                    services.AddSingleton(
                        new Claim(
                            JwtClaimTypes.Subject,
                            userId.ToString()));
                }

                _testServer = TestServerFactory.Create(Configure);
            }

            return _testServer;
        }
    }
}
