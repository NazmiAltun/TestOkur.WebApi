namespace TestOkur.Report.Integration.Tests.Common
{
    using System.Security.Claims;
    using IdentityModel;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using TestOkur.TestHelper;

    public abstract class Test
    {
        private static readonly TestServerFactory TestServerFactory = new TestServerFactory();
        private TestServer _testServer;

        public TestServer Create() => Create(RandomGen.Next(1000));

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
