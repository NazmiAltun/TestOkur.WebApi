namespace TestOkur.WebApi.Integration.Tests.Score
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Score;
	using Xunit;

	public class EditTests : ScoreTest
	{
		protected const float BasePoint = 50;
		protected const float Coefficient = 0.5f;

		[Fact]
		public async Task Should_CreateExamScoreFormula_When_NotExists()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var createExamCommand = await CreateExamAsync(client);
				var examId = (await GetExamListAsync(client)).First().Id;
				var scoreFormula = (await GetScoreFormulaList(client)).Random();
				var coefficients = new Dictionary<int, float>();

				foreach (var coef in scoreFormula.Coefficients)
				{
					coefficients.Add(coef.LessonCoefficientId, Coefficient);
				}

				var command = new SaveExamScoreFormulaCommand(examId, scoreFormula.Id, Random.Next(1000), coefficients);
				var response = await client.PostAsync(ApiPath, command.ToJsonContent());
				response.EnsureSuccessStatusCode();
				var list = await GetScoreFormulaList(client);
				var examScore = list.FirstOrDefault(s => s.ExamId == examId);
				examScore.Should().NotBeNull();
				examScore.BasePoint.Should().Be(command.BasePoint);
				examScore.Coefficients.Select(c => c.Coefficient)
					.Should().AllBeEquivalentTo(Coefficient);
			}
		}

		[Fact]
		public async Task FormulaShouldBeUpdated()
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
			await client.PutAsync(ApiPath, command.ToJsonContent());
		}
	}
}
