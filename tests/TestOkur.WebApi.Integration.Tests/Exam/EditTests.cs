namespace TestOkur.WebApi.Integration.Tests.Exam
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Contracts.Exam;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Exam.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class EditTests : ExamTest
    {
        [Fact]
        public async Task WhenNotExistingValuesPosted_Then_ShouldEdit()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                var command = await CreateExamAsync(client);
                var exams = await GetExamListAsync(client);
                var id = exams.First(e => e.Name == command.Name).Id;
                var examType = await GetRandomExamTypeAsync(client);

                var editCommand = new EditExamCommand(
                    Guid.NewGuid(),
                    id,
                    Random.RandomString(100),
                    DateTime.Today.AddDays(10),
                    examType.Id,
                    Random.Next(4),
                    examType.OpticalFormTypes.Random().Code,
                    Random.Next(1) + 1,
                    Random.Next(9) + 1,
                    Random.Next(3) + 1,
                    null,
                    Random.RandomString(300));
                var response = await client.PutAsync(ApiPath, editCommand.ToJsonContent());
                response.EnsureSuccessStatusCode();
                exams = await GetExamListAsync(client);
                exams.Should().Contain(e => e.Name == editCommand.NewName &&
                                            e.ExamDate == editCommand.NewExamDate &&
                                            e.ExamTypeId == editCommand.NewExamTypeId &&
                                            e.IncorrectEliminationRate == editCommand.NewIncorrectEliminationRate &&
                                            e.ApplicableFormTypeCode == editCommand.NewApplicableFormTypeCode &&
                                            e.ExamBookletTypeId == editCommand.NewExamBookletTypeId &&
                                            e.LessonId == editCommand.NewLessonId &&
                                            e.Notes == editCommand.NewNotes);
                exams.Should().NotContain(e => e.Name == command.Name);

                var @event = Consumer.Instance.GetFirst<IExamUpdated>();
                @event.ExamId.Should().Be(editCommand.ExamId);
            }
        }
    }
}
