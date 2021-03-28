namespace TestOkur.WebApi.Integration.Tests
{
    using FluentAssertions;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class HealthCheckTests : IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public HealthCheckTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }


        [Fact]
        public async Task HealthCheckEndpointShouldWork_WhenServerIsRunning()
        {
            var client = _webApplicationFactory.CreateClient();
            var response = await client.GetAsync("hc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
        }
    }
}
