namespace TestOkur.Notification.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using TestOkur.Notification.Models;

	public class WebApiClient : IWebApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly OAuthClient _identityServerClient;

		public WebApiClient(
			HttpClient httpClient,
			OAuthClient identityServerClient)
		{
			_httpClient = httpClient;
			_identityServerClient = identityServerClient;
		}

		public async Task DeductSmsCreditsAsync(int userId, string smsBody)
		{
			var model = new DeductSmsCreditsModel
			{
				Id = Guid.NewGuid(),
				LicenseId = userId,
				SmsBody = smsBody,
			};
			await SetBearerToken();
			var response = await _httpClient.PostAsync("/api/v1/sms/deduct-credits", model.ToJsonContent());
			response.EnsureSuccessStatusCode();
		}

		public async Task<AppSettingReadModel> GetAppSettingAsync(string name)
		{
			return (await GetAsync<IEnumerable<AppSettingReadModel>>("/api/v1/settings/appsettings"))
				.First(t => t.Name == name);
		}

		private async Task<TModel> GetAsync<TModel>(string requestUri)
		{
			await SetBearerToken();
			var response = await _httpClient.GetAsync(requestUri);
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<TModel>(json);
		}

		private async Task SetBearerToken()
		{
			_httpClient.SetBearerToken(await _identityServerClient.GetToken());
		}
	}
}
