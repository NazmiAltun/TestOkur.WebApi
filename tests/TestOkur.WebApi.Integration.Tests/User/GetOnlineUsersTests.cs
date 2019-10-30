namespace TestOkur.WebApi.Integration.Tests.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class GetOnlineUsersTests : UserTest
    {
        [Fact]
        public async Task When_There_Are_Online_Users_Then_Email_List_Of_Online_Users_Should_Return()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            await client.GetAsync($"{ApiPath}/me");
            var response = await client.GetAsync($"{ApiPath}/online");
            var onlineUsers = await response.ReadAsync<IEnumerable<string>>();
            onlineUsers.Should().NotBeNullOrEmpty();
        }
    }
}
