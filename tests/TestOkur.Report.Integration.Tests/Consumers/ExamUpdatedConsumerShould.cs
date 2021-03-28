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
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class ExamUpdatedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public ExamUpdatedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task UpdateAnswerKeyForms(IFixture fixture, int userId)
        {
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();

            var examId = await ExecuteExamCreatedConsumerAsync(repo, fixture);
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
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
            var consumer = new ExamUpdatedConsumer(repo, null);
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
