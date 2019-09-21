namespace TestOkur.WebApi.Integration.Tests
{
    using System.Net;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class PrometheusTests : Test
    {
        [Fact]
        public async Task PrometheusMetricEndpointShouldReturnResult()
        {
            var response = await (await GetTestServer()).CreateClient().GetAsync("metrics-core");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("text/plain");
        }
    }
}
