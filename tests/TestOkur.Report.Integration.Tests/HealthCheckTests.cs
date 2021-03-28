using System.Net.Mime;

namespace TestOkur.Report.Integration.Tests
{
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Report.Integration.Tests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class HealthCheckTests : IClassFixture<WebApplicationFactory>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly WebApplicationFactory _webApplicationFactory;

        public HealthCheckTests(
            ITestOutputHelper testOutputHelper,
            WebApplicationFactory webApplicationFactory)
        {
            _testOutputHelper = testOutputHelper;
            _webApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var client = _webApplicationFactory.CreateClient();
            var response = await client.GetAsync("hc");
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        }
    }
}
