namespace TestOkur.WebApi.Integration.Tests.User
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Contracts.User;
	using TestOkur.Data;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class ActivateUserTests : UserTest
	{
		[Fact]
		public async Task When_ValidValues_Are_Posted_Then_User_Should_Be_Activated_And_Event_Published()
		{
			const string email = "nazmialtun@windowslive.com";

			using (var testServer = await CreateAsync())
			{
				var client = testServer.CreateClient();
				var response = await client.PostAsync($"{ApiPath}/activate?email={email}", null);
				response.EnsureSuccessStatusCode();

				var dbContext = testServer.Host.Services.GetService(typeof(ApplicationDbContext))
					as ApplicationDbContext;
				var user = await dbContext.Users.FirstAsync(u => u.Email == email);
				var @event = Consumer.Instance.GetFirst<IUserActivated>();
				@event.Email.Should().Be(user.Email);
			}
		}
	}
}
