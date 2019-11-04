namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Report.Integration.Tests.Common;
    using TestOkur.Report.Models;
    using TestOkur.TestHelper;
    using Xunit;

    public class AddTests : Test
    {
        private const string ApiPath = "api/v1/report-requests";

        [Fact]
        public async Task ShouldAddRequests()
        {
            var userId = RandomGen.Next(10000);

            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            var request = new ReportRequest()
            {
                Booklet = "A",
                Classroom = "All",
                ExamId = RandomGen.Next(),
                ExamName = RandomGen.String(100),
                ExportType = RandomGen.String(100),
                Id = Guid.NewGuid(),
                ReportType = RandomGen.String(100),
                RequestDateTimeUtc = DateTime.UtcNow.AddSeconds(-2),
                ResponseDateTimeUtc = DateTime.UtcNow.AddSeconds(1),
                UserId = userId.ToString(),
                UserEmail = RandomGen.String(100),
            };
            var response = await client.PostAsync(ApiPath, request.ToJsonContent());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
