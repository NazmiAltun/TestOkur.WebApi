namespace TestOkur.WebApi.Integration.Tests.Score
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Score;
	using TestOkur.WebApi.Integration.Tests.Exam;

	public abstract class ScoreTest : ExamTest
	{
		protected new const string ApiPath = "api/v1/score-formulas";

		protected async Task<IEnumerable<ScoreFormulaReadModel>> GetScoreFormulaList(HttpClient client)
		{
			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<ScoreFormulaReadModel>>();
		}

		protected void ShouldContainTrialScoreFormulas(
			IEnumerable<ScoreFormulaReadModel> formulas)
		{
			var expectedList = new[]
			{
				new { Grade =2, BasePoint=200f },
				new { Grade =3, BasePoint=200f },
				new { Grade =4, BasePoint=200f },
				new { Grade =5, BasePoint=194.760f },
				new { Grade =6, BasePoint=194.760f },
				new { Grade =7, BasePoint=194.760f },
				new { Grade =8, BasePoint=194.760f },
			};
			foreach (var expected in expectedList)
			{
				formulas.Should()
					.Contain(f => f.FormulaType == FormulaType.Trial.Name &&
								  f.FormulaTypeId == FormulaType.Trial.Id &&
								  f.Grade == expected.Grade &&
								  f.BasePoint == expected.BasePoint);
			}
		}

		protected void ShouldContainTytScoreFormulas(
			IEnumerable<ScoreFormulaReadModel> formulas)
		{
			formulas.Should()
				.Contain(f => f.FormulaType == FormulaType.TytAyt.Name &&
							  f.FormulaTypeId == FormulaType.TytAyt.Id &&
							  f.ScoreName == FormulaNames.Tyt);
			formulas.Should()
				.Contain(f => f.FormulaType == FormulaType.TytAyt.Name &&
							  f.FormulaTypeId == FormulaType.TytAyt.Id &&
							  f.ScoreName == FormulaNames.AytEa);
			formulas.Should()
				.Contain(f => f.FormulaType == FormulaType.TytAyt.Name &&
							  f.FormulaTypeId == FormulaType.TytAyt.Id &&
							  f.ScoreName == FormulaNames.AytLang);
			formulas.Should()
				.Contain(f => f.FormulaType == FormulaType.TytAyt.Name &&
							  f.FormulaTypeId == FormulaType.TytAyt.Id &&
							  f.ScoreName == FormulaNames.AytSay);
			formulas.Should()
				.Contain(f => f.FormulaType == FormulaType.TytAyt.Name &&
							  f.FormulaTypeId == FormulaType.TytAyt.Id &&
							  f.ScoreName == FormulaNames.AytSoz);
		}

		protected void ShouldContainScholarshipExamTypeFormulas(
			IEnumerable<ScoreFormulaReadModel> formulas)
		{
			var expectedList = new[]
			{
				new { Grade =4, BasePoint=175 },
				new { Grade =5, BasePoint=175 },
				new { Grade =6, BasePoint=175 },
				new { Grade =7, BasePoint=129 },
				new { Grade =8, BasePoint=129 },
				new { Grade =9, BasePoint=200 },
			};
			var lessons = new[]
			{
				Lessons.Turkish, Lessons.Mathematics, Lessons.Science, Lessons.SocialScience,
			};

			formulas.Should()
				.Contain(f => f.FormulaTypeId == FormulaType.Scholarship.Id &&
							  f.ScoreName == "Lise");
			foreach (var expected in expectedList)
			{
				formulas.Should()
					.Contain(f => f.FormulaType == FormulaType.Scholarship.Name &&
								  f.FormulaTypeId == FormulaType.Scholarship.Id &&
								  f.Grade == expected.Grade &&
								  f.BasePoint == expected.BasePoint);

				formulas.First(f => f.FormulaTypeId == FormulaType.Scholarship.Id &&
									f.Grade == expected.Grade)
					.Coefficients.Select(c => c.Lesson)
					.Should().Contain(lessons);
			}
		}
	}
}
