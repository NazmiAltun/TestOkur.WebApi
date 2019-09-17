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
    using TestOkur.Report.Repositories;
    using TestOkur.TestHelper;
    using Xunit;

    public class SubjectChangedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task ChangeSubjectsOfQuestions()
        {
            var userId = RandomGen.Next(10000);
            var subjectId = RandomGen.Next();
            var subject = RandomGen.String(40);
            var newSubject = RandomGen.String(40);
            var answerKeyForms = SetSubject(
                GenerateAnswerKeyOpticalForms(1).ToList(),
                subjectId,
                subject);

            using (var testServer = Create(userId))
            {
                var client = testServer.CreateClient();
                var examId = await ExecuteExamCreatedConsumerAsync(testServer, answerKeyForms);
                var repository = testServer.Host.Services.GetService(typeof(IOpticalFormRepository));
                var consumer = new SubjectChangedConsumer(repository as IOpticalFormRepository);
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
