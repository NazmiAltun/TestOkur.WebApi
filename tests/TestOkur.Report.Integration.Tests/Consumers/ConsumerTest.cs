namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using MassTransit;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Report.Integration.Tests.OpticalForm;

    public abstract class ConsumerTest : OpticalFormTest
    {
        protected Task<int> ExecuteExamCreatedConsumerAsync(IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository, IFixture fixture)
        {
            return ExecuteExamCreatedConsumerAsync(
                answerKeyOpticalFormRepository,
                GenerateAnswerKeyOpticalForms(fixture, 4).ToList(),
                fixture.Create<int>());
        }

        protected async Task<int> ExecuteExamCreatedConsumerAsync(
            IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository,
            List<AnswerKeyOpticalForm> answerKeyOpticalForms,
            int examId)
        {
            var consumer = new ExamCreatedConsumer(answerKeyOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IExamCreated>>();
            context.Message.ExamId.Returns(examId);
            context.Message.AnswerKeyOpticalForms.Returns(answerKeyOpticalForms);
            await consumer.Consume(context);

            return examId;
        }
    }
}
