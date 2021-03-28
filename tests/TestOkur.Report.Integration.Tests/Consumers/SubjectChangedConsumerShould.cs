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
    using TestOkur.Test.Common;
    using Xunit;

    public class SubjectChangedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public SubjectChangedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ChangeSubjectsOfQuestions(IFixture fixture, int subjectId, string subject, int userId, string newSubject, int examId)
        {
            var answerKeyForms = SetSubject(
                GenerateAnswerKeyOpticalForms(fixture, 1).ToList(),
                subjectId,
                subject);

            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var answerKeyOpticalFormRepository = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();

            await ExecuteExamCreatedConsumerAsync(answerKeyOpticalFormRepository, answerKeyForms, examId);
            var repository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
                
            var consumer = new SubjectChangedConsumer(repository, answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<ISubjectChanged>>();
            context.Message.SubjectId.Returns(subjectId);
            context.Message.NewName.Returns(newSubject);
            await consumer.Consume(context);
            var answerKeyOpticalForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            answerKeyOpticalForms.SelectMany(a => a.Sections)
                .SelectMany(s => s.Answers)
                .Select(a => a.SubjectName)
                .Distinct()
                .ToList()
                .Should()
                .HaveCount(1)
                .And
                .Contain(newSubject);
        }

        private List<AnswerKeyOpticalForm> SetSubject(List<AnswerKeyOpticalForm> forms, int subjectId, string subjectName)
        {
            foreach (var form in forms)
            {
                foreach (var section in form.Sections)
                {
                    foreach (var question in section.Answers)
                    {
                        question.SubjectId = subjectId;
                        question.SubjectName = subjectName;
                    }
                }
            }

            return forms;
        }
    }
}
