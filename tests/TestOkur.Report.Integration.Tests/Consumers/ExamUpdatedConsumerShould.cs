namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.TestHelper;
    using Xunit;

    public class ExamUpdatedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task UpdateAnswerKeyForms()
        {
            var userId = RandomGen.Next(10000);
            using (var testServer = Create(userId))
            {
                var examId = await ExecuteExamCreatedConsumerAsync(testServer);
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
                studentForms.First().SetFromScanOutput(new ScanOutput("BABABABACADADADADACA", 0), list.First());
                studentForms.Last().SetFromScanOutput(new ScanOutput("ABCABDABCABDAC", 0), list.Last());
                await client.PostAsync(ApiPath, studentForms.ToJsonContent());
                var newAnswerKeyForms = GenerateAnswerKeyOpticalForms(4).ToList();
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
}
