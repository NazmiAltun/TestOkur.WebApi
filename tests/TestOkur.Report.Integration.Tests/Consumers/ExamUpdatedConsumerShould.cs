namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class ExamUpdatedConsumerShould : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task UpdateAnswerKeyForms(IFixture fixture, int userId)
        {
            using var testServer = Create(userId);
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, fixture);
            var client = testServer.CreateClient();
            var list = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            list.Should().NotBeEmpty();
            var studentForms = new List<StudentOpticalForm>()
            {
                new StudentOpticalForm(list.First().Booklet)
                {
                    SchoolId = userId,
                    UserId = userId.ToString(),
                    ExamId = examId,
                },
                new StudentOpticalForm(list.Last().Booklet)
                {
                    SchoolId = userId,
                    UserId = userId.ToString(),
                    ExamId = examId,
                },
            };
            studentForms.First().SetFromScanOutput(new ScanOutput("BABABABACADADADADACA", 0, 0, 'A'), list.First());
            studentForms.Last().SetFromScanOutput(new ScanOutput("ABCABDABCABDAC", 0, 0, 'A'), list.Last());
            await client.PostAsync(ApiPath, studentForms.ToJsonContent());
            var newAnswerKeyForms = GenerateAnswerKeyOpticalForms(fixture, 4).ToList();
            var repository = testServer.Host.Services.GetService(typeof(IAnswerKeyOpticalFormRepository));
            var consumer = new ExamUpdatedConsumer(repository as IAnswerKeyOpticalFormRepository, null);
            var context = Substitute.For<ConsumeContext<IExamUpdated>>();
            context.Message.ExamId.Returns(examId);
            context.Message.AnswerKeyOpticalForms.Returns(newAnswerKeyForms);
            await consumer.Consume(context);
            list = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            list.Should().HaveCount(newAnswerKeyForms.Count);
            studentForms = (await GetListAsync<StudentOpticalForm>(client, examId)).ToList();
            studentForms.Should().NotBeEmpty();
        }
    }
}
