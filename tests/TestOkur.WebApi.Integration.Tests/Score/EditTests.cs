namespace TestOkur.WebApi.Integration.Tests.Score
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Serialization;
    using TestOkur.WebApi.Application.Score;
    using Xunit;

    public class EditTests : ScoreTest
    {
        protected const float BasePoint = 50;
        protected const float Coefficient = 0.5f;

        [Fact]
        public async Task FormulaShouldBeUpdated()
        {
            using var testServer = await CreateWithUserAsync();
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
        }

        protected async Task EditScoreFormulaAsync(HttpClient client, IEnumerable<ScoreFormulaReadModel> formulas)
        {
            var commandList = new List<EditScoreFormulaCommand>();

            foreach (var formula in formulas)
            {
                var coefficients = new Dictionary<int, float>();

                foreach (var coef in formula.Coefficients)
                {
                    coefficients.Add(coef.LessonCoefficientId, Coefficient);
                }

                commandList.Add(new EditScoreFormulaCommand(
                    formula.Id,
                    BasePoint,
                    coefficients));
            }

            var command = new BulkEditScoreFormulaCommand(commandList);
            var response = await client.PutAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
