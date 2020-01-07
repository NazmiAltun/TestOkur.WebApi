namespace TestOkur.WebApi.Application.User.Clients
{
    using IdentityModel.Client;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Serializer;

    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;
        private readonly OAuthConfiguration _oAuthConfiguration;

        public IdentityClient(
            HttpClient httpClient,
            OAuthConfiguration oAuthConfiguration)
        {
            _httpClient = httpClient;
            _oAuthConfiguration = oAuthConfiguration;
        }

        public async Task DeleteUserAsync(string id, CancellationToken cancellationToken)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.DeleteAsync($"/api/v1/users/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task ExtendUserSubscriptionAsync(string id, CancellationToken cancellationToken)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PostAsync($"/api/v1/users/extend?id={id}", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task ActivateUserAsync(string email, CancellationToken cancellationToken)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PostAsync($"/api/v1/users/activate?email={email}", null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RegisterUserAsync(CreateCustomerUserModel model, CancellationToken cancellationToken = default)
        {
            await SetBearerTokenAsync();
            model.MaxAllowedDeviceCount = 1000;
            var response = await _httpClient.PostAsync("/api/v1/users/create", model.ToJsonContent(), cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(UpdateUserModel model, CancellationToken cancellationToken = default)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PostAsync("/api/v1/users/update", model.ToJsonContent(), cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PostAsync(
                $"/api/v1/users/generate-password-reset-token?email={email}",
                null,
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ValidationException(ErrorCodes.PasswordResetUserNotFound);
            }

            return await JsonUtils.DeserializerFromHttpContentAsync<string>(
                response.Content,
                cancellationToken);
        }

        public async Task<string> GetBearerTokenAsync()
        {
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = $"{_oAuthConfiguration.Authority}connect/token",
                    ClientId = Clients.Private,
                    ClientSecret = _oAuthConfiguration.PrivateClientSecret,
                    Scope = _oAuthConfiguration.ApiName,
                });

            return tokenResponse.AccessToken;
        }

        private async Task SetBearerTokenAsync()
        {
            _httpClient.SetBearerToken(await GetBearerTokenAsync());
        }
    }
}
