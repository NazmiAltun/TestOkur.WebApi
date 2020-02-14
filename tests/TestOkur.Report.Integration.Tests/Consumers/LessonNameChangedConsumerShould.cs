namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.TestHelper;
    using Xunit;
    using TestOkur.Serialization;

    public class LessonNameChangedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task UpdateLessonNamesOfOpticalForms()
        {
            var lessonId = RandomGen.Next();
            var lessonName = RandomGen.String(20);
            var answerKeyForms = GenerateAnswerKeyOpticalForms(3, lessonId, lessonName).ToList();
            var userId = RandomGen.Next(10000);

            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, answerKeyForms);
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(examId, userId, lessonId, lessonName),
                GenerateStudentForm(examId, userId, lessonId, lessonName),
                GenerateStudentForm(examId, userId, lessonId, lessonName),
                GenerateStudentForm(examId, userId, lessonId, lessonName),
                GenerateStudentForm(examId, userId, lessonId, lessonName),
                GenerateStudentForm(examId, userId, lessonId, lessonName),
            };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.SelectMany(f => f.Sections)
                .Should().NotContain(s => s.LessonName != lessonName);

            var newLessonName = RandomGen.String(20);
            var repository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository))
                as IStudentOpticalFormRepository;
            var answerKeyOpticalFormRepository = testServer.Host.Services.GetService(typeof(IAnswerKeyOpticalFormRepository))
                as IAnswerKeyOpticalFormRepository;
            var consumer = new LessonNameChangedConsumer(repository, answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<ILessonNameChanged>>();
            context.Message.LessonId.Returns(lessonId);
            context.Message.NewLessonName.Returns(newLessonName);
            await consumer.Consume(context);
            studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.SelectMany(f => f.Sections)
                .Select(s => s.LessonName)
                .Distinct()
                .Should()
                .HaveCount(1)
                .And
                .Contain(newLessonName);
            var answerKeyOpticalForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            answerKeyOpticalForms.SelectMany(f => f.Sections)
                .Select(s => s.LessonName)
                .Distinct()
                .Should()
                .HaveCount(1)
                .And
                .Contain(newLessonName);
        }
    }
}
