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
    using TestOkur.Report.Integration.Tests.OpticalForm;
    using TestOkur.Report.Repositories;
    using TestOkur.TestHelper;

    public abstract class ConsumerTest : OpticalFormTest
    {
        protected async Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer)
        {
            return await ExecuteExamCreatedConsumerAsync(
                testServer,
                GenerateAnswerKeyOpticalForms(4).ToList());
        }

        protected async Task<int> ExecuteExamCreatedConsumerAsync(TestServer testServer, List<AnswerKeyOpticalForm> answerKeyOpticalForms)
        {
            var examId = RandomGen.Next();
            var repository = testServer.Host.Services.GetService(typeof(IOpticalFormRepository));
            var consumer = new ExamCreatedConsumer(repository as IOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IExamCreated>>();
            context.Message.ExamId.Returns(examId);
            context.Message.AnswerKeyOpticalForms.Returns(answerKeyOpticalForms);
            await consumer.Consume(context);

            return examId;
        }
    }
}
