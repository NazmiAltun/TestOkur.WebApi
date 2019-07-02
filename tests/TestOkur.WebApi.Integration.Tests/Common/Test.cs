namespace TestOkur.WebApi.Integration.Tests.Common
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using CacheManager.Core;
	using IdentityModel;
	using Microsoft.AspNetCore.TestHost;
	using Microsoft.Extensions.DependencyInjection;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Captcha;
	using TestOkur.WebApi.Application.City;
	using TestOkur.WebApi.Application.LicenseType;
	using TestOkur.WebApi.Application.User.Commands;

	public abstract class Test
	{
		protected static readonly Random Random = new Random();
		private static readonly TestServerFactory TestServerFactory = new TestServerFactory();
		private TestServer _testServerWithUser;
		private TestServer _testServerWithoutUser;

		public async Task<TestServer> CreateAsync()
		{
			if (_testServerWithoutUser == null)
			{
				_testServerWithoutUser = await TestServerFactory.CreateAsync();
			}

			return _testServerWithoutUser;
		}

		public async Task<TestServer> CreateAsync(Action<IServiceCollection> configureServices)
			=> await TestServerFactory.CreateAsync(configureServices);

		public async Task<TestServer> CreateWithUserAsync()
		{
			if (_testServerWithUser == null)
			{
				var userId = await CreateUserAsync(CreateAsync);

				void Configure(IServiceCollection services)
				{
					services.AddSingleton(new Claim(JwtClaimTypes.Subject, userId));
				}

				_testServerWithUser = await CreateAsync(Configure);
			}

			return _testServerWithUser;
		}

		protected async Task<string> CreateUserAsync(Func<Task<TestServer>> testServerFactory)
		{
			using (var testServer = await testServerFactory())
			{
				var client = testServer.CreateClient();
				var captcha = await GetCaptchaAsync(client, testServer.Host.Services);
				var city = await GetRandomCityAsync(client);
				var licenseType = await GetRandomLicenseTypeAsync(client);
				var model = GenerateCreateUserCommand(captcha, city, licenseType);

				var response = await client.PostAsync("api/v1/users", model.ToJsonContent());
				response.EnsureSuccessStatusCode();

				return model.Id.ToString();
			}
		}

		protected async Task<LicenseTypeReadModel> GetRandomLicenseTypeAsync(HttpClient client)
		{
			var response = await client.GetAsync("/api/v1/license-types");
			var licenseTypes = await response.ReadAsync<IEnumerable<LicenseTypeReadModel>>();

			return licenseTypes.Random();
		}

		protected async Task<CityReadModel> GetRandomCityAsync(HttpClient client)
		{
			var response = await client.GetAsync("/api/v1/cities");
			var cities = await response.ReadAsync<IEnumerable<CityReadModel>>();

			return cities.Random();
		}

		protected async Task<Captcha> GetCaptchaAsync(HttpClient client, IServiceProvider serviceProvider)
		{
			var id = Guid.NewGuid();
			await client.GetAsync($"/api/v1/captcha/{id}");
			var cache = serviceProvider.GetRequiredService<ICacheManager<Captcha>>();

			return cache.Get($"Captcha_{id}");
		}

		protected CreateUserCommand GenerateCreateUserCommand(
			Captcha captcha = null,
			CityReadModel city = null,
			LicenseTypeReadModel licenseType = null,
			string phone = null)
		{
			var district = city?.Districts?.Random();

			return new CreateUserCommand(
				Guid.NewGuid(),
				"Jack",
				Random.Next(int.MaxValue / 2, int.MaxValue).ToString(),
				"Jack",
				"Black",
				"AS",
				$"{Random.RandomString(120)}@hotmail.com",
				phone ?? Random.Next(int.MaxValue / 2, int.MaxValue).ToString(),
				Random.Next(99999, int.MaxValue).ToString(),
				licenseType?.Id ?? 1,
				city?.Id ?? 0,
				district?.Id ?? 0,
				licenseType?.Name ?? "test",
				city?.Name ?? "test",
				district?.Name ?? "test",
				captcha?.Id ?? Guid.Empty,
				captcha?.Code ?? "NONONO");
		}
	}
}
