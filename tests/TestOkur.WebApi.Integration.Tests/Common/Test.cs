namespace TestOkur.WebApi.Integration.Tests.Common
{
    using CacheManager.Core;
    using IdentityModel;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.City;
    using TestOkur.WebApi.Application.User.Commands;

    public abstract class Test
    {
        protected static readonly Random Random = new Random();
        protected static readonly TestServerFactory TestServerFactory = new TestServerFactory();

        private const string UserApiPath = "api/v1/users";
        private const string CitiesApiPath = "api/v1/cities";
        private const string CaptchaApiPath = "api/v1/captcha";
        private const string LicenseTypesApiPath = "api/v1/license-types";
        private static TestServer _testServer;
        private TestServer _testServerWithUser;

        public async Task<TestServer> CreateAsync(Action<IServiceCollection> configureServices)
            => await TestServerFactory.CreateAsync(configureServices);

        public async Task<TestServer> CreateWithUserAsync()
        {
            if (_testServerWithUser == null)
            {
                var userId = (await CreateUserAsync(CreateAsync)).Id.ToString();

                void Configure(IServiceCollection services)
                {
                    services.AddSingleton(new Claim(JwtClaimTypes.Subject, userId));
                }

                _testServerWithUser = await CreateAsync(Configure);
            }

            return _testServerWithUser;
        }

        protected static async Task<TestServer> GetTestServer()
        {
            if (_testServer != null)
            {
                return _testServer;
            }

            _testServer = await TestServerFactory.CreateAsync();
            return _testServer;
        }

        protected async Task<CreateUserCommand> CreateUserAsync(Func<Task<TestServer>> testServerFactory)
        {
            using (var testServer = await testServerFactory())
            {
                var client = testServer.CreateClient();
                var captcha = await GetCaptchaAsync(client, testServer.Host.Services);
                var city = await GetRandomCityAsync(client);
                var model = GenerateCreateUserCommand(captcha, city);

                var response = await client.PostAsync(UserApiPath, model.ToJsonContent());
                response.EnsureSuccessStatusCode();

                return model;
            }
        }

        protected async Task<CityReadModel> GetRandomCityAsync(HttpClient client)
        {
            var response = await client.GetAsync(CitiesApiPath);
            var cities = await response.ReadAsync<IEnumerable<CityReadModel>>();

            return cities.Random();
        }

        protected async Task<Captcha> GetCaptchaAsync(HttpClient client, IServiceProvider serviceProvider)
        {
            var id = Guid.NewGuid();
            await client.GetAsync($"{CaptchaApiPath}/{id}");
            var cache = serviceProvider.GetRequiredService<ICacheManager<Captcha>>();

            return cache.Get($"Captcha_{id}");
        }

        protected CreateUserCommand GenerateCreateUserCommand(
            Captcha captcha = null,
            CityReadModel city = null,
            string phone = null)
        {
            var district = city?.Districts?.Random();

            return new CreateUserCommand(
                Guid.NewGuid(),
                "Jack",
                RandomGen.Phone(),
                "Jack",
                "Black",
                "AS",
                $"{Random.RandomString(120)}@hotmail.com",
                phone ?? RandomGen.Phone(),
                Random.Next(99999, int.MaxValue).ToString(),
                1 + Random.Next(4),
                city?.Id ?? 0,
                district?.Id ?? 0,
                "test",
                city?.Name ?? "test",
                district?.Name ?? "test",
                captcha?.Id ?? Guid.Empty,
                captcha?.Code ?? "NONONO");
        }

        private async Task<TestServer> CreateAsync()
        {
            return await TestServerFactory.CreateAsync();
        }
    }
}
