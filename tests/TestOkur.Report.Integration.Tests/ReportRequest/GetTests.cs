namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using AutoFixture;
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Report.Models;
    using TestOkur.Test.Common;
    using TestOkur.Test.Common.Extensions;
    using Xunit;

    public class GetTests : ReportRequestTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task ShouldGetRequests(IFixture fixture, int userId)
        {
            using var testServer = Create(userId);
            var client = testServer.CreateClient();

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
