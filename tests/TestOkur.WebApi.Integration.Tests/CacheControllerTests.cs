namespace TestOkur.WebApi.Integration.Tests
{
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class CacheControllerTests : Test
    {
        private const string ApiPath = "api/cache";
        private const string Key = "dummy-key";

        [Fact(Skip = "Fix later")]
        public async Task When_Key_Not_Provided_Should_Return_BadRequest()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.DeleteAsync(ApiPath);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact (Skip = "Fix later")]
        public async Task When_InvalidKey_Provided_Should_Return_UnauthorizedResult()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.DeleteAsync($"{ApiPath}?key=1324");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(Skip = "Fix later")]
        public async Task When_Key_Provided_Then_StatusCode_Should_Be_Ok()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.DeleteAsync($"{ApiPath}?key={Key}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
