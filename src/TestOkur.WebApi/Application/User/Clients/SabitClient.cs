namespace TestOkur.WebApi.Application.User.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Newtonsoft.Json;

    public class SabitClient : ISabitClient
    {
        private const string LicenseTypesEndpoint = "api/v1/license-types";

        private readonly HttpClient _httpClient;
        private readonly IIdentityClient _identityClient;

        public SabitClient(HttpClient httpClient, IIdentityClient identityClient)
        {
            _httpClient = httpClient;
            _identityClient = identityClient;
        }

        public async Task<IEnumerable<LicenseType>> GetLicenseTypesAsync()
        {
            _httpClient.SetBearerToken(await _identityClient.GetBearerTokenAsync());
            var response = await _httpClient.GetAsync(LicenseTypesEndpoint);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<LicenseType>>(json);
        }
    }
}