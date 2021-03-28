using System.Net.Http;
using System.Threading.Tasks;
using TestOkur.Report.Integration.Tests.Common;
using TestOkur.Serialization;
using Xunit;

namespace TestOkur.Report.Integration.Tests.ReportRequest
{
    using AutoFixture;
    using TestOkur.Report.Models;

    public abstract class ReportRequestTest
    {
        private const string ApiPath = "api/v1/report-requests";

        protected Task<HttpResponseMessage> AddRandomAsync(HttpClient client, IFixture fixture, int userId)
        {
            var request = fixture.Create<ReportRequest>();
            request.UserId = userId.ToString();

            return client.PostAsync(ApiPath, request.ToJsonContent());
        }

        protected Task<HttpResponseMessage> GetAsync(HttpClient client)
        {
            return client.GetAsync(ApiPath);
        }
    }
}