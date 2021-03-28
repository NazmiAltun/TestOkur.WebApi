using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Infrastructure.Repositories;
using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using FluentAssertions;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Test.Common;
    using Xunit;

    public class ExamCreatedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public ExamCreatedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task PersistAnswerKeyForms_When_ValidMessagePassed(IFixture fixture)
        {
            using var client = _webApplicationFactory.CreateClientWithUserId(fixture.Create<int>());
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            var examId = await ExecuteExamCreatedConsumerAsync(repo, fixture);
            var list = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            list.Should().NotBeEmpty();
        }
    }
}
