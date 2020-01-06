namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using TestOkur.Notification.Models;
    using TestOkur.Serializer;

    public class WebApiClient : IWebApiClient
    {
        private const string AppSettingsEndpoint = "/api/v1/settings/appsettings";
        private const string UsersEndpoint = "api/v1/users";
        private const string StatisticsEndpoint = "api/v1/statistics";

        private readonly HttpClient _httpClient;
        private readonly IOAuthClient _oAuthClient;

        public WebApiClient(
            HttpClient httpClient,
            IOAuthClient oAuthClient)
        {
            _httpClient = httpClient;
            _oAuthClient = oAuthClient;
        }

        public Task<StatisticsReadModel> GetStatisticsAsync()
        {
            return GetAsync<StatisticsReadModel>(StatisticsEndpoint);
        }

        public async Task ReEvaluateAllExamsAsync()
        {
            await SetBearerToken();
            var response = await _httpClient.PostAsync("api/v1/exams/re-evaluate", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<AppSettingReadModel> GetAppSettingAsync(string name)
        {
            return (await GetAsync<IEnumerable<AppSettingReadModel>>(AppSettingsEndpoint))
                .First(t => t.Name == name);
        }

        public Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return GetAsync<IEnumerable<UserModel>>(UsersEndpoint);
        }

        private async Task<TModel> GetAsync<TModel>(string requestUri)
        {
            await SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await JsonUtils.DeserializerFromHttpContent<TModel>(response.Content);
        }

        private async Task SetBearerToken()
        {
            _httpClient.SetBearerToken(await _oAuthClient.GetTokenAsync());
        }
    }
}
