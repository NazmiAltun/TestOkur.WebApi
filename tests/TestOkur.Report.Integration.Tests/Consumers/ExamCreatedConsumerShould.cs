namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Test.Common;
    using Xunit;

    public class ExamCreatedConsumerShould : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task PersistAnswerKeyForms_When_ValidMessagePassed(IFixture fixture)
        {
            using var testServer = Create(fixture.Create<int>());
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, fixture);
            var list = await GetListAsync<AnswerKeyOpticalForm>(testServer.CreateClient(), examId);
            list.Should().NotBeEmpty();
        }
    }
}
