namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using Xunit;

    public class ExamDeletedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task DeleteOpticalForms()
        {
            using var testServer = Create();
            var examId = await ExecuteExamCreatedConsumerAsync(testServer);
            var list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
            list.Should().NotBeEmpty();
            var studentOpticalFormRepository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository))
                as IStudentOpticalFormRepository;
            var answerKeyOpticalFormRepository = testServer.Host.Services.GetService(typeof(IAnswerKeyOpticalFormRepository))
                as IAnswerKeyOpticalFormRepository;
            var consumer = new ExamDeletedConsumer(studentOpticalFormRepository, answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IExamDeleted>>();
            context.Message.ExamId.Returns(examId);
            await consumer.Consume(context);
            list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
            list.Should().BeEmpty();
        }
    }
}
