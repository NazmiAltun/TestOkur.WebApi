namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.TestHelper;
    using Xunit;

    public class AddTests : ReportRequestTest
    {
        [Fact]
        public async Task ShouldAddRequests()
        {
            var userId = RandomGen.Next(10000);

            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            var response = await AddRandomAsync(client, userId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
