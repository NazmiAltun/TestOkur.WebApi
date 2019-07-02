namespace TestOkur.WebApi.Integration.Tests.Score
{
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Integration.Tests.Exam;

	public abstract class ScoreTest : ExamTest
	{
		protected new const string ApiPath = "api/v1/score-formulas";

		protected async Task<IEnumerable<ScoreFormulaReadModel>> GetScoreFormulaList(HttpClient client)
		{
			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<ScoreFormulaReadModel>>();
		}
	}
}
