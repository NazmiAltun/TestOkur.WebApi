namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class GetTests : UnitTest
    {
       [Fact(Skip = "Fix later")]
        public async Task Should_Return_SharedUnitsAndSubjects()
        {
            using var testServer = await CreateWithUserAsync();
            var units = await GetUnitListAsync(testServer.CreateClient());
            units.Should().NotBeEmpty();
            units.All(u => u.Shared).Should().BeTrue();
            units.SelectMany(u => u.Subjects)
                .All(s => s.Shared)
                .Should().BeTrue();
        }
    }
}
