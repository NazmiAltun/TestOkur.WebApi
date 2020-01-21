using System.Net.Mime;

namespace TestOkur.Notification.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;
    using Xunit.Abstractions;

    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public HealthCheckTests(WebApplicationFactory<Startup> factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await _factory.CreateClient().GetAsync("hc");
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
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
