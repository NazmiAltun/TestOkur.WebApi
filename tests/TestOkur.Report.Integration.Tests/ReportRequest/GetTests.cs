namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Report.Models;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class GetTests : ReportRequestTest
    {
        [Fact]
        public async Task ShouldGetRequests()
        {
            var userId = RandomGen.Next(10000);

            using var testServer = Create(userId);
            var client = testServer.CreateClient();

            for (var i = 0; i < 100; i++)
            {
                await AddRandomAsync(client, userId);
            }

            var response = await GetAsync(client);
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.ReadAsync<ReportStatisticsModel>();
            result.TotalCount.Should().BeGreaterOrEqualTo(100);
        }
    }
}
