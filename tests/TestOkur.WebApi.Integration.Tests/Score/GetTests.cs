namespace TestOkur.WebApi.Integration.Tests.Score
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.Model.ScoreModel;
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
