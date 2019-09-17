namespace TestOkur.WebApi.Integration.Tests.User
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class DeleteUserTests : UserTest
	{
		[Fact]
		public async Task UserShouldBeDeleted()
		{
			using (var testServer = await CreateAsync())
			{
				var client = testServer.CreateClient();
				var model = await CreateUserAsync(client, testServer.Host.Services);
				var user = (await GetUsersAsync(client)).FirstOrDefault(u => u.Email == model.Email);
				user.Should().NotBeNull();
				await client.DeleteAsync($"{ApiPath}/{user.Id}");
				(await GetUsersAsync(client)).Should().NotContain(u => u.Email == user.Email ||
				                               u.Id == user.Id);
			}
		}
	}
}
