namespace TestOkur.WebApi.Integration.Tests
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Settings;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class SettingsControllerTests : Test
    {
        private const string ApiPath = "api/v1/settings";

        [Fact]
        public async Task Given_Get_When_Requested_Then_ShouldReturn_Seeded_AppSettings()
        {
            using (var testServer = await CreateAsync())
            {
                var response = await testServer.CreateClient().GetAsync($"{ApiPath}/appsettings");
                var settings = await response.ReadAsync<IEnumerable<AppSettingReadModel>>();
                settings.Should().NotBeEmpty();
                settings.Should().NotContain(t => string.IsNullOrEmpty(t.Name) ||
                                                   string.IsNullOrEmpty(t.Value));
            }
        }
    }
}
