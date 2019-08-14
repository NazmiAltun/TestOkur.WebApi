namespace TestOkur.WebApi.Integration.Tests
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Localization;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class LocalizationApiTests : Test
	{
		private const string ApiPath = "api/v1/localization";

		[Fact]
		public async Task Should_Return_LocalStrings_When_CultureCodeProvided()
		{
			using (var testServer = await CreateAsync())
			{
				var client = testServer.CreateClient();
				var response = await client.GetAsync($"{ApiPath}/tr");
				var strings = await response.ReadAsync<IEnumerable<LocalString>>();
				strings.Should().NotBeEmpty();
			}
		}
	}
}
