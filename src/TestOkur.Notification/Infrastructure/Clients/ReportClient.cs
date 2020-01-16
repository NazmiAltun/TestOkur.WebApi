namespace TestOkur.Notification.Infrastructure.Clients
{
    using IdentityModel.Client;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;
    using TestOkur.Serialization;

    public class ReportClient : IReportClient
    {
        private const string StatisticsEndpoint = "api/v1/report-requests";

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
