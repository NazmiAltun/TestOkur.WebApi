namespace TestOkur.WebApi.Integration.Tests.User
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Common;
    using TestOkur.Contracts.User;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.User.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class ResetPasswordTests : UserTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("nope")]
        public async Task WhenInvalidEmailPosted_ThenInvalidEmailError_Should_Return(string email)
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();

            var command = new SendResetPasswordLinkCommand(
                Guid.NewGuid(), email, Guid.NewGuid(), Random.RandomString(4));

            var response = await client.PostAsync($"{ApiPath}/send-reset-password-link", command.ToJsonContent());
            await response.Should().BeBadRequestAsync(ErrorCodes.InvalidEmailAddress);
        }

        [Fact]
        public async Task WhenValidEmailPosted_Then_LinkShouldBeSentToUser()
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();
            var model = await CreateUserAsync(client);

            var command = new SendResetPasswordLinkCommand(
                Guid.NewGuid(), model.Email, Guid.NewGuid(), Random.RandomString(4));

            var response = await client.PostAsync($"{ApiPath}/send-reset-password-link", command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            var events = Consumer.Instance.GetAll<IResetPasswordTokenGenerated>();
            events.Should().Contain(e => e.Email == command.Email &&
                                         e.FirstName == model.UserFirstName &&
                                         e.LastName == model.UserLastName &&
                                         !string.IsNullOrEmpty(e.Link));
        }
    }
}
