namespace TestOkur.Notification.Infrastructure.Clients
{
    using IdentityModel.Client;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;
    using TestOkur.Serialization;

    public class ReportClient : IReportClient
    {
        private const string StatisticsEndpoint = "api/v1/report-requests";
        private const string ExamStatisticsEndpoint = "api/v1/exam-statistics/multiple";

        private readonly HttpClient _httpClient;
        private readonly IOAuthClient _oAuthClient;

        public ReportClient(
            HttpClient httpClient,
            IOAuthClient oAuthClient)
        {
            _httpClient = httpClient;
            _oAuthClient = oAuthClient;
        }

        public Task<ReportStatisticsModel> GetStatisticsAsync()
        {
            return GetAsync<ReportStatisticsModel>(StatisticsEndpoint);
        }

        public Task<IEnumerable<ExamStatistics>> GetExamStatisticsAsync(IEnumerable<int> examIds)
        {
            return GetAsync<IEnumerable<ExamStatistics>>($"{ExamStatisticsEndpoint}/{string.Join(",", examIds)}");
        }

        private async Task<TModel> GetAsync<TModel>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return await JsonUtils.DeserializerFromHttpContentAsync<TModel>(response.Content);
        }

        private async Task SetBearerToken()
        {
            _httpClient.SetBearerToken(await _oAuthClient.GetTokenAsync());
        }
    }
}
