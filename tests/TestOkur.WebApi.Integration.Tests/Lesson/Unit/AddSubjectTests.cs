namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Serializer;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using Xunit;

    public class AddSubjectTests : UnitTest
    {
        [Fact]
        public async Task Given_AddSubject_When_NameIsEmpty_Then_Server_Should_ReturnBadRequest()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            await CreateUnitAsync(client);
            var units = await GetUnitListAsync(client);
            var addSubjectCommand = new AddSubjectCommand(
                Guid.NewGuid(),
                string.Empty,
                units.Last().Id);
            var response = await client.PostAsync(
                $"{ApiPath}/{addSubjectCommand.UnitId}/subjects",
                addSubjectCommand.ToJsonContent());
            await response.Should()
                .BeBadRequestAsync(ErrorCodes.NameCannotBeEmpty);
        }

        [Fact]
        public async Task Given_AddSubject_When_ValidModelPosted_Then_Server_Should_AddSubject()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var addSubject = await AddSubjectAsync(client);
            var units = await GetUnitListAsync(client);
            units.First(u => u.Id == addSubject.UnitId)
                .Subjects.Should()
                .Contain(s => s.Name == addSubject.Name);
        }
    }
}
