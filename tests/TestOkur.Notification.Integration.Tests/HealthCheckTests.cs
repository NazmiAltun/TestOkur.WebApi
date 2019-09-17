namespace TestOkur.Notification.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Notification.Integration.Tests.Common;
    using Xunit;

    public class HealthCheckTests
    {
        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await TestServerFactory.TestServer.CreateClient().GetAsync("hc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task EndPoint_For_HangfireDashboard_Should_Work()
        {
            var response = await TestServerFactory.TestServer.CreateClient()
                .GetAsync("hangfire?username=user&password=pass");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
