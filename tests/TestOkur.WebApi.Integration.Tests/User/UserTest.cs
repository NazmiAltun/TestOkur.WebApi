namespace TestOkur.WebApi.Integration.Tests.User
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using TestOkur.WebApi.Application.User.Commands;
	using TestOkur.WebApi.Integration.Tests.Common;

	public abstract class UserTest : Test
	{
		protected const string ApiPath = "api/v1/users";

		protected async Task<CreateUserCommand> CreateUserAsync(
			HttpClient client,
			IServiceProvider serviceProvider)
		{
			var captcha = await GetCaptchaAsync(client, serviceProvider);
			var city = await GetRandomCityAsync(client);
			var licenseType = await GetRandomLicenseTypeAsync(client);
			var command = GenerateCreateUserCommand(captcha, city, licenseType);
			var response = await client.PostAsync(ApiPath, command.ToJsonContent());
			response.EnsureSuccessStatusCode();

			return command;
		}
	}
}
