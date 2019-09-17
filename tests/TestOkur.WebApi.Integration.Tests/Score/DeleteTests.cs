namespace TestOkur.WebApi.Integration.Tests.Score
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class DeleteTests : EditTests
    {
        [Fact]
        public async Task UsersScoreFormulasShouldBeDeleted()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                var formulas = await GetScoreFormulaList(client);
                await EditScoreFormulaAsync(client, formulas);
                formulas = await GetScoreFormulaList(client);

                foreach (var formula in formulas)
                {
                    formula.BasePoint.Should().Be(BasePoint);
                    formula.Coefficients
                        .Select(c => c.Coefficient)
                        .Should()
                        .AllBeEquivalentTo(Coefficient);
                }

                await client.DeleteAsync(ApiPath);
                formulas = await GetScoreFormulaList(client);
                ShouldContainScholarshipExamTypeFormulas(formulas);
                ShouldContainTytScoreFormulas(formulas);
                ShouldContainTrialScoreFormulas(formulas);
            }
        }
    }
}
