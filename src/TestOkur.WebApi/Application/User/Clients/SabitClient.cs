namespace TestOkur.WebApi.Application.User.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using TestOkur.Common;

    public class SabitClient : ISabitClient
    {
        private const string LicenseTypesEndpoint = "api/v1/license-types";
        private const string CitiesEndpoint = "api/v1/cities";

        private readonly HttpClient _httpClient;
        private readonly IIdentityClient _identityClient;

        public SabitClient(HttpClient httpClient, IIdentityClient identityClient)
        {
            _httpClient = httpClient;
            _identityClient = identityClient;
        }

        public Task<IEnumerable<LicenseType>> GetLicenseTypesAsync() => GetAsync<IEnumerable<LicenseType>>(LicenseTypesEndpoint);

        public Task<IEnumerable<City>> GetCitiesAsync() => GetAsync<IEnumerable<City>>(CitiesEndpoint);

        private async Task<T> GetAsync<T>(string path)
        {
            _httpClient.SetBearerToken(await _identityClient.GetBearerTokenAsync());
            var response = await _httpClient.GetAsync(path);
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json, DefaultJsonSerializerSettings.Instance);
        }
    }
}