namespace TestOkur.WebApi.Integration.Tests.Classroom
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Common;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class CreateTests : ClassroomTest
    {
        [Fact]
        public async Task When_Classroom_Exists_Then_BadRequestShouldBeReturned()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                var command = await CreateClassroomAsync(client);
                var response = await client.PostAsync(ApiPath, command.ToJsonContent());
                await response.Should().BeBadRequestAsync(ErrorCodes.ClassroomExists);
            }
        }

        [Fact]
        public async Task When_ValidValuesArePosted_Then_ClassroomShouldBeCreated()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                var command = await CreateClassroomAsync(client);
                (await GetListAsync(client)).Should()
                    .Contain(l => l.Grade == command.Grade && l.Name == command.Name);
            }
        }
    }
}
