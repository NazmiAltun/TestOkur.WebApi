namespace TestOkur.WebApi.Integration.Tests.Lesson
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Serialization;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class EditTests : LessonTest
    {
        [Fact]
        public async Task When_ValidValuesPosted_Then_ShouldBeEdited()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command1 = await CreateLessonAsync(client);
            var list = await GetLessonListAsync(client);
            var id = list.First(c => c.Name == command1.Name).Id;
            var editCommand = new EditLessonCommand(Guid.NewGuid(), id, $"{command1.Name}_New");
            var response = await client.PutAsync(ApiPath, editCommand.ToJsonContent());
            response.EnsureSuccessStatusCode();
            list = await GetLessonListAsync(client);
            list.Should().Contain(l => l.Name == editCommand.NewName)
                .And.NotContain(l => l.Name == command1.Name);

            var @event = Consumer.Instance.GetFirst<ILessonNameChanged>();
            @event.NewLessonName.Should().Be(editCommand.NewName);
        }

        [Fact]
        public async Task When_ExistingValuePosted_Then_BadRequestShouldBeReturned()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command1 = await CreateLessonAsync(client);
            var command2 = await CreateLessonAsync(client);
            var list = await GetLessonListAsync(client);
            var id = list.First(c => c.Name == command1.Name).Id;
            var editCommand = new EditLessonCommand(Guid.NewGuid(), id, command2.Name);
            var response = await client.PutAsync(ApiPath, editCommand.ToJsonContent());
            await response.Should().BeBadRequestAsync(ErrorCodes.LessonExists);
        }
    }
}
