namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using AutoFixture;
    using FluentAssertions;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Test.Common;
    using Xunit;

    public class AddTests : ReportRequestTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task ShouldAddRequests(IFixture fixture, int userId)
        {
            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            var response = await AddRandomAsync(client, fixture, userId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
