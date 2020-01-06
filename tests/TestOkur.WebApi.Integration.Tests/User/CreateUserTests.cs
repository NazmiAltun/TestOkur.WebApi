namespace TestOkur.WebApi.Integration.Tests.User
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Contracts.User;
    using TestOkur.Serializer;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class CreateUserTests : UserTest
    {
        
        [Fact]
        public async Task When_BadDataPosted_Then_BadRequest_Should_Return()
        {
            var client = (await GetTestServer()).CreateClient();
            var model = GenerateCreateUserCommand(Random.RandomString(5));

            var response = await client.PostAsync(ApiPath, model.ToJsonContent());
            await response.Should().BeBadRequestAsync(ErrorCodes.InvalidPhoneNumber);
        }

        [Fact]
        public async Task When_ValidValues_Are_Posted_Then_User_Should_Be_Created_And_Event_Published()
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();
            var model = await CreateUserAsync(client);
            (await GetUsersAsync(client)).Should()
                    .Contain(u => u.Email == model.Email);
            var events = Consumer.Instance.GetAll<INewUserRegistered>();
            events.Should().Contain(e => e.Email == model.Email);
        }

        [Fact]
        public async Task When_UserAlreadyExists_Then_BadRequest_Code_Should_Return()
        {
            var testServer = await GetTestServer();
            var client = testServer.CreateClient();
            var model = await CreateUserAsync(client);
            var response = await client.PostAsync(ApiPath, model.ToJsonContent());
            await response.Should().BeBadRequestAsync(ErrorCodes.UserAlreadyExists);
        }
    }
}