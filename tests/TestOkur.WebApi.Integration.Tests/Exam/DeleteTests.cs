namespace TestOkur.WebApi.Integration.Tests.Exam
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Contracts.Exam;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class DeleteTests : ExamTest
    {
        [Fact]
        public async Task ShouldDeleteAndPublishEvent()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command = await CreateExamAsync(client);
            var id = (await GetExamListAsync(client))
                .First(l => l.Name == command.Name).Id;
            await DeleteExamAsync(client, id);
            (await GetExamListAsync(client)).Should()
                .NotContain(l => l.Name == command.Name);
            var @event = Consumer.Instance.GetFirst<IExamDeleted>();
            @event.ExamId.Should().Be(id);
        }
    }
}
