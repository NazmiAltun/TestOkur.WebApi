using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using AutoFixture;
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Test.Common;
    using Xunit;

    public class AddTests : ReportRequestTest , IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public AddTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldAddRequests(IFixture fixture, int userId)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var response = await AddRandomAsync(client, fixture, userId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
