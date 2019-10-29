namespace TestOkur.Sabit.Integration.Tests
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Sabit.Application.Localization;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class LocalizationTests : IClassFixture<WebApplicationFactory>
    {
        private const string ApiPath = "api/v1/localization";

        private readonly WebApplicationFactory _factory;

        public LocalizationTests(WebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_Get_When_Requested_Then_ShouldReturn_Seeded_LocalStrings()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(ApiPath);
            var strings = await response.ReadAsync<IEnumerable<LocalString>>();
            strings.Should().NotBeEmpty();
        }
    }
}
