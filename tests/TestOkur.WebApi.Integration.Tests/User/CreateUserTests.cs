namespace TestOkur.WebApi.Integration.Tests.User
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Common;
    using TestOkur.Contracts.User;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class CreateUserTests : UserTest
    {
        [Fact]
        public async Task When_InvalidCaptchaPosted_Then_BadRequest_Should_Return()
        {
            using (var testServer = await CreateAsync())
            {
                var client = testServer.CreateClient();
                var model = GenerateCreateUserCommand();
                var response = await client.PostAsync(ApiPath, model.ToJsonContent());
                await response.Should().BeBadRequestAsync(ErrorCodes.InvalidCaptcha);
            }
        }

        [Fact]
        public async Task When_BadDataPosted_Then_BadRequest_Should_Return()
        {
            using (var testServer = await CreateAsync())
            {
                var client = testServer.CreateClient();
                var model = GenerateCreateUserCommand(null, null, null, string.Empty);

                var response = await client.PostAsync(ApiPath, model.ToJsonContent());
                await response.Should().BeBadRequestAsync(ErrorCodes.InvalidPhoneNumber);
            }
        }

        [Fact]
        public async Task When_ValidValues_Are_Posted_Then_User_Should_Be_Created_And_Event_Published()
        {
            using (var testServer = await CreateAsync())
            {
                var client = testServer.CreateClient();
                var model = await CreateUserAsync(client, testServer.Host.Services);
                (await GetUsersAsync(client)).Should()
                    .Contain(u => u.Email == model.Email);
                var events = Consumer.Instance.GetAll<INewUserRegistered>();
                events.Should().Contain(e => e.Email == model.Email);
            }
        }

        [Fact]
        public async Task When_UserAlreadyExists_Then_BadRequest_Code_Should_Return()
        {
            using (var testServer = await CreateAsync())
            {
                var client = testServer.CreateClient();
                var model = await CreateUserAsync(client, testServer.Host.Services);
                var response = await client.PostAsync(ApiPath, model.ToJsonContent());
                await response.Should().BeBadRequestAsync(ErrorCodes.UserAlreadyExists);
            }
        }
    }
}