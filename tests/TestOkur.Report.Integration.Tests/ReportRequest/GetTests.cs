using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using AutoFixture;
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Report.Models;
    using TestOkur.Test.Common;
    using TestOkur.Test.Common.Extensions;
    using Xunit;

    public class GetTests : ReportRequestTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public GetTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldGetRequests(IFixture fixture, int userId)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(userId);

            for (var i = 0; i < 100; i++)
            {
                await AddRandomAsync(client, fixture, userId);
            }

            var response = await GetAsync(client);
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.ReadAsync<ReportStatisticsModel>();
            result.TotalCount.Should().BeGreaterOrEqualTo(100);
        }
    }
}
