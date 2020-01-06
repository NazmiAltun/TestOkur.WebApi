namespace TestOkur.WebApi.Integration.Tests.User
{
    using FluentAssertions;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using TestOkur.Serializer;
    using TestOkur.WebApi.Application.User.Commands;
    using Xunit;

    public class UpdateUserTests : UserTest
    {
        [Fact]
        public async Task WhenValidValuesPosted_ThenUserShouldBeUpdated()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var updateCommand = new UpdateUserCommand(
                Guid.NewGuid(),
                "BlackSchool",
                "5324258289",
                81,
                23);
            var response = await client.PutAsync(ApiPath, updateCommand.ToJsonContent());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
