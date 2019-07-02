namespace TestOkur.Notification.Infrastructure
{
	using System.Net.Http;
	using System.Threading.Tasks;
	using IdentityModel.Client;
	using TestOkur.Common.Configuration;

	public class OAuthClient
	{
		private readonly HttpClient _httpClient;
		private readonly OAuthConfiguration _oAuthConfiguration;

		public OAuthClient(
			OAuthConfiguration oAuthConfiguration,
			HttpClient httpClient)
		{
			_oAuthConfiguration = oAuthConfiguration;
			_httpClient = httpClient;
		}

		public async Task<string> GetToken()
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
	}
}
