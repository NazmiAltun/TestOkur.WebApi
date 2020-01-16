namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Serialization;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class EditSubjectTests : UnitTest
    {
        [Fact]
        public async Task Given_EditSubject_When_ValidModelPosted_Then_Server_Should_UpdateSubject()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var addSubjectCommand = await AddSubjectAsync(client);
            var units = await GetUnitListAsync(client);
            var subject = units.First(u => u.Id == addSubjectCommand.UnitId)
                .Subjects.First(s => s.Name == addSubjectCommand.Name);
            var editSubjectCommand = new EditSubjectCommand(
                Guid.NewGuid(),
                subject.Id,
                subject.Name + "_NEW");
            await client.PutAsync(
                $"{ApiPath}/{addSubjectCommand.UnitId}/subjects",
                editSubjectCommand.ToJsonContent());
            units = await GetUnitListAsync(client);
            units.Should().Contain(u => u.Id == addSubjectCommand.UnitId &&
                                        u.Subjects.Any(s => s.Name == editSubjectCommand.NewName));
            units.Should().NotContain(u => u.Id == addSubjectCommand.UnitId &&
                                           u.Subjects.Any(s => s.Name == addSubjectCommand.Name));

            var @event = Consumer.Instance.GetFirst<ISubjectChanged>();
            @event.NewName.Should().Be(editSubjectCommand.NewName);
        }
    }
}
