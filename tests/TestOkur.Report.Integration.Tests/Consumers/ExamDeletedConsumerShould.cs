namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Exam;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Repositories;
    using Xunit;

    public class ExamDeletedConsumerShould : ConsumerTest
	{
		[Fact]
		public async Task DeleteOpticalForms()
		{
			using (var testServer = Create())
			{
				var examId = await ExecuteExamCreatedConsumerAsync(testServer);
				var list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
				list.Should().NotBeEmpty();
				var repository = testServer.Host.Services.GetService(typeof(IOpticalFormRepository));
				var consumer = new ExamDeletedConsumer(repository as IOpticalFormRepository);
				var context = Substitute.For<ConsumeContext<IExamDeleted>>();
				context.Message.ExamId.Returns(examId);
				await consumer.Consume(context);
				list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
				list.Should().BeEmpty();
			}
		}
	}
}
