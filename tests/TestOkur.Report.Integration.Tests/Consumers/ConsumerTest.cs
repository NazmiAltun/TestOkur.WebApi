namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.AspNetCore.TestHost;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Report.Integration.Tests.OpticalForm;
    using TestOkur.TestHelper;

    public abstract class ConsumerTest : OpticalFormTest
    {
        protected Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer)
        {
            return ExecuteExamCreatedConsumerAsync(
                testServer,
                GenerateAnswerKeyOpticalForms(4).ToList());
        }

        protected async Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer, List<AnswerKeyOpticalForm> answerKeyOpticalForms)
        {
            var examId = RandomGen.Next();
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
