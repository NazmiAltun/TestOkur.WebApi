using System.Net.Mime;

namespace TestOkur.WebApi.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class HealthCheckTests : Test
    {
        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await (await GetTestServer()).CreateClient().GetAsync("hc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        }
    }
}
