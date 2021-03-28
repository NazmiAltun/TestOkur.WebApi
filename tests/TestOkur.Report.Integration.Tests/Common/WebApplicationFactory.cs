using System.Net.Http;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TestOkur.Report.Integration.Tests.Common
{
    public class WebApplicationFactory : WebApplicationFactory<Startup>
    {
        public HttpClient CreateClientWithUserId(int userId)
        {
            return WithWebHostBuilder(x =>
            {
                x.ConfigureTestServices(services =>
                {
                    services.AddSingleton(
                        new Claim(
                            JwtClaimTypes.Subject,
                            userId.ToString()));
                });
            }).CreateClient();
        }
    }
}