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

    public class ExamDeletedConsumerShould : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task DeleteOpticalForms(IFixture fixture)
        {
            using var testServer = Create(fixture.Create<int>());
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, fixture);
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
