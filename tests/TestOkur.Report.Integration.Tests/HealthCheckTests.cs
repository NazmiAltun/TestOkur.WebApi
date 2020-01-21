using System.Net.Mime;

namespace TestOkur.Report.Integration.Tests
{
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Report.Integration.Tests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class HealthCheckTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public HealthCheckTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await TestServerFactory.TestServer.CreateClient().GetAsync("hc");
            _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        }
    }
}
