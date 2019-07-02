namespace TestOkur.WebApi.Integration.Tests.User
{
	using System;
	using System.Threading.Tasks;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.User.Commands;
	using Xunit;

	public class UpdateUserTests : UserTest
	{
		[Fact]
		public async Task WhenValidValuesPosted_ThenUserShouldBeUpdated()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var city = await GetRandomCityAsync(client);
				var updateCommand = new UpdateUserCommand(
					Guid.NewGuid(),
					"BlackSchool",
					"5324258289",
					city.Id,
					city.Districts.Random().Id);
				var response = await client.PutAsync(ApiPath, updateCommand.ToJsonContent());
				response.EnsureSuccessStatusCode();
			}
		}
	}
}
