namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Newtonsoft.Json;
    using TestOkur.Common.Configuration;
    using TestOkur.Notification.Models;

    public class OAuthClient : IOAuthClient
    {
        private const string UsersEndpoint = "api/v1/users";
        private const string StatsEndpoint = "api/v1/stats";
        private readonly HttpClient _httpClient;
        private readonly OAuthConfiguration _oAuthConfiguration;

        public OAuthClient(
            OAuthConfiguration oAuthConfiguration,
            HttpClient httpClient)
        {
            _oAuthConfiguration = oAuthConfiguration;
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            return (await _httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = $"{_oAuthConfiguration.Authority}connect/token",
                    ClientId = TestOkur.Common.Clients.Private,
                    ClientSecret = _oAuthConfiguration.PrivateClientSecret,
                    Scope = _oAuthConfiguration.ApiName,
                })).AccessToken;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsersAsync()
        {
            _httpClient.SetBearerToken(await GetTokenAsync());
            var response = await _httpClient.GetAsync(UsersEndpoint);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<IdentityUser>>(json);
        }

        public async Task<IdentityStatisticsModel> GetDailyStatsAsync()
        {
            _httpClient.SetBearerToken(await GetTokenAsync());
            var response = await _httpClient.GetAsync(StatsEndpoint);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IdentityStatisticsModel>(json);
        }
    }
}
