namespace TestOkur.WebApi.Application.User.Services
{
	using System;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using CacheManager.Core;
	using IdentityModel.Client;
	using TestOkur.Common;
	using TestOkur.Common.Configuration;
	using TestOkur.Domain.Model;

	public class IdentityService : IIdentityService
	{
		private const string CacheKey = "private_access_token";
		private readonly HttpClient _httpClient;
		private readonly OAuthConfiguration _oAuthConfiguration;
		private readonly ICacheManager<string> _tokenCache;

		public IdentityService(
			HttpClient httpClient,
			OAuthConfiguration oAuthConfiguration,
			ICacheManager<string> tokenCache)
		{
			_httpClient = httpClient;
			_oAuthConfiguration = oAuthConfiguration;
			_tokenCache = tokenCache;
		}

		public async Task ActivateUserAsync(Email email, CancellationToken cancellationToken)
		{
			await SetBearerToken();
			var response = await _httpClient.PostAsync($"/account/activate?email={email}", null, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		public async Task RegisterUserAsync(CreateCustomerUserModel model, CancellationToken cancellationToken = default)
		{
			await SetBearerToken();
			var response = await _httpClient.PostAsync("/account/create", model.ToJsonContent(), cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		public async Task<string> GeneratePasswordResetTokenAsync(Email email, CancellationToken cancellationToken = default)
		{
			await SetBearerToken();
			var response = await _httpClient.PostAsync(
				$"/account/generate-password-reset-token?email={email}",
				null,
				cancellationToken);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}

		private async Task SetBearerToken()
		{
			var token = _tokenCache.Get(CacheKey);

			if (string.IsNullOrEmpty(token))
			{
				var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
					new ClientCredentialsTokenRequest()
					{
						Address = $"{_oAuthConfiguration.Authority}connect/token",
						ClientId = Clients.Private,
						ClientSecret = _oAuthConfiguration.PrivateClientSecret,
						Scope = _oAuthConfiguration.ApiName,
					});
				token = tokenResponse.AccessToken;
				StoreAccessTokenInCache(token);
			}

			_httpClient.SetBearerToken(token);
		}

		private void StoreAccessTokenInCache(string token)
		{
			_tokenCache.Add(new CacheItem<string>(
				CacheKey,
				token,
				ExpirationMode.Absolute,
				TimeSpan.FromMinutes(59)));
		}
	}
}
