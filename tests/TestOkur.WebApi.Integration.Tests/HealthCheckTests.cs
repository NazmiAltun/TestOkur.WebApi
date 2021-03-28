namespace TestOkur.WebApi.Integration.Tests
{
    using FluentAssertions;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class HealthCheckTests : Test
    {
        [Fact(Skip = "Fix later")]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var response = await (await GetTestServer()).CreateClient().GetAsync("hc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        }
    }
}
