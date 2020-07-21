namespace TestOkur.Sabit.Integration.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using TestOkur.Common;

    public class WebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(
                        AuthorizationPolicies.Public,
                        policy => policy.RequireAssertion(context => true));
                });
            });
        }
    }
}