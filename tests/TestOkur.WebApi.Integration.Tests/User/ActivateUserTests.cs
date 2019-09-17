namespace TestOkur.WebApi.Integration.Tests.User
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Contracts.User;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class ActivateUserTests : UserTest
	{
		[Fact]
		public async Task When_ValidValues_Are_Posted_Then_User_Should_Be_Activated_And_Event_Published()
		{
            var command = await CreateUserAsync(CreateAsync);

            using (var testServer = await CreateAsync())
			{
				var client = testServer.CreateClient();
				await client.PostAsync($"{ApiPath}/activate?email={command.Email}", null);
				var user = (await GetUsersAsync(client)).First(u => u.Email == command.Email);
				var @event = Consumer.Instance.GetFirst<IUserActivated>();
				@event.Email.Should().Be(user.Email);
			}
		}
	}
}
