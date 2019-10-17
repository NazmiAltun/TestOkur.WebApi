namespace TestOkur.Notification.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await _factory.CreateClient().GetAsync("hc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task EndPoint_For_HangfireDashboard_Should_Work()
        {
            var response = await _factory.CreateClient()
                .GetAsync("hangfire?username=user&password=pass");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
