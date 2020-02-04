using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestOkur.Report.Integration.Tests.Common;
using TestOkur.Serialization;
using TestOkur.TestHelper;

namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    public abstract class ReportRequestTest : Test
    {
        private const string ApiPath = "api/v1/report-requests";

        protected Task<HttpResponseMessage> AddRandomAsync(HttpClient client, int userId)
        {
            return client.PostAsync(ApiPath, CreateRandomRequest(userId).ToJsonContent());
        }

        protected Task<HttpResponseMessage> GetAsync(HttpClient client)
        {
            return client.GetAsync(ApiPath);
        }

        private Models.ReportRequest CreateRandomRequest(int userId)
        {
            return new Models.ReportRequest()
            {
                Booklet = RandomGen.String(1),
                Classroom = RandomGen.String(3),
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
        }
    }
}