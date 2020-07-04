namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using MassTransit;
    using Microsoft.AspNetCore.TestHost;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Report.Integration.Tests.OpticalForm;

    public abstract class ConsumerTest : OpticalFormTest
    {
        protected Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer, IFixture fixture)
        {
            return ExecuteExamCreatedConsumerAsync(
                testServer,
                GenerateAnswerKeyOpticalForms(fixture, 4).ToList(),
                fixture.Create<int>());
        }

        protected async Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer, List<AnswerKeyOpticalForm> answerKeyOpticalForms, int examId)
        {
            var answerKeyOpticalFormRepository = testServer.Host.Services.GetService(typeof(IAnswerKeyOpticalFormRepository))
                as IAnswerKeyOpticalFormRepository;
            var consumer = new ExamCreatedConsumer(answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IExamCreated>>();
            context.Message.ExamId.Returns(examId);
            context.Message.AnswerKeyOpticalForms.Returns(answerKeyOpticalForms);
            await consumer.Consume(context);

            return examId;
        }
    }
}
