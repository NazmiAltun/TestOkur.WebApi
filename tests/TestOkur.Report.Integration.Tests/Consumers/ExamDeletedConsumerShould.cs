using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Threading.Tasks;
    using AutoFixture;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Test.Common;
    using Xunit;

    public class ExamDeletedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public ExamDeletedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task DeleteOpticalForms(IFixture fixture)
        {
            var client = _webApplicationFactory.CreateClient();
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            var examId = await ExecuteExamCreatedConsumerAsync(repo, fixture);
            var list = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            list.Should().NotBeEmpty();
            var studentOpticalFormRepository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var answerKeyOpticalFormRepository = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            var consumer = new ExamDeletedConsumer(studentOpticalFormRepository, answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IExamDeleted>>();
            context.Message.ExamId.Returns(examId);
            await consumer.Consume(context);
            list = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            list.Should().BeEmpty();
        }
    }
}
