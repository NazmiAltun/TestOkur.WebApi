namespace TestOkur.WebApi.Integration.Tests.Common
{
    using CacheManager.Core;
    using IdentityModel;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.User.Commands;

    public abstract class Test
    {
        protected static readonly Random Random = new Random();
        protected static readonly TestServerFactory TestServerFactory = new TestServerFactory();

        private const string UserApiPath = "api/v1/users";
        private const string CaptchaApiPath = "api/v1/captcha";
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
            using var testServer = await testServerFactory();
            var client = testServer.CreateClient();
            var captcha = await GetCaptchaAsync(client, testServer.Host.Services);
            var model = GenerateCreateUserCommand(captcha);

            var response = await client.PostAsync(UserApiPath, model.ToJsonContent());
            response.EnsureSuccessStatusCode();

            return model;
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
            string phone = null)
        {
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
                20,
                466,
                "test",
                Random.RandomString(10),
                Random.RandomString(10),
                captcha?.Id ?? Guid.Empty,
                captcha?.Code ?? "NONONO");
        }

        private Task<TestServer> CreateAsync()
        {
            return TestServerFactory.CreateAsync();
        }
    }
}
