namespace TestOkur.WebApi.Integration.Tests.Score
{
    using System.Threading.Tasks;
    using Xunit;

    public class GetTests : ScoreTest
    {
        [Fact]
        public async Task ShouldReturnScoreFormulas()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                var formulas = await GetScoreFormulaList(client);
                ShouldContainScholarshipExamTypeFormulas(formulas);
                ShouldContainTytScoreFormulas(formulas);
                ShouldContainTrialScoreFormulas(formulas);
            }
        }
    }
}
