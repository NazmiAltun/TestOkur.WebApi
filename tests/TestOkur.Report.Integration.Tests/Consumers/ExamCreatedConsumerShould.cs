namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Optic.Form;
    using Xunit;

    public class ExamCreatedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task PersistAnswerKeyForms_When_ValidMessagePassed()
        {
            using (var testServer = Create())
            {
                var examId = await ExecuteExamCreatedConsumerAsync(testServer);
                var list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
                list.Should().NotBeEmpty();
            }
        }
    }
}
