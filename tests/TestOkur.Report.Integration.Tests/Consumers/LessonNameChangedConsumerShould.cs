using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Lesson;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class LessonNameChangedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public LessonNameChangedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task UpdateLessonNamesOfOpticalForms(IFixture fixture, int userId, int lessonId, string lessonName, string newLessonName, int examId)
        {
            var answerKeyForms = GenerateAnswerKeyOpticalForms(fixture, 3, lessonId, lessonName).ToList();
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            await ExecuteExamCreatedConsumerAsync(repo, answerKeyForms, examId);
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

            var repository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var consumer = new LessonNameChangedConsumer(repository, repo);
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
