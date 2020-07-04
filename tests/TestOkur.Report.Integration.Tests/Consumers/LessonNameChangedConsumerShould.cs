namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using Xunit;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;

    public class LessonNameChangedConsumerShould : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task UpdateLessonNamesOfOpticalForms(IFixture fixture, int userId, int lessonId, string lessonName, string newLessonName, int examId)
        {
            var answerKeyForms = GenerateAnswerKeyOpticalForms(fixture, 3, lessonId, lessonName).ToList();
            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            await ExecuteExamCreatedConsumerAsync(testServer, answerKeyForms, examId);
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
                GenerateStudentForm(fixture,examId, userId, lessonId, lessonName),
            };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);

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
                .Contain(newLessonName);
            var answerKeyOpticalForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            answerKeyOpticalForms.SelectMany(f => f.Sections)
                .Select(s => s.LessonName)
                .Distinct()
                .Should()
                .Contain(newLessonName);
        }
    }
}
