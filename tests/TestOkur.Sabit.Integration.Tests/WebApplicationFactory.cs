namespace TestOkur.Sabit.Integration.Tests
{
    using MassTransit.RabbitMqTransport;
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
                var host = services.BuildServiceProvider()
                    .GetRequiredService<IRabbitMqHost>();
                host.ConnectReceiveEndpoint("test", x => Consumer.Instance.Configure(x));
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