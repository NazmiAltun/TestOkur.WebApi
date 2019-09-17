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
            using (var testServer = await CreateAsync())
            {
                var response = await testServer.CreateClient().GetAsync("hc");
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            }
        }
    }
}
