namespace TestOkur.WebApi.Application.Score
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using TestOkur.Domain.Model.ScoreModel;

	[DebuggerDisplay("{FormulaType}-{ScoreName}")]
	public class ScoreFormulaReadModel
	{
		public ScoreFormulaReadModel()
		{
			Coefficients = new List<LessonCoefficientReadModel>();
		}

		public int Id { get; set; }

		public float BasePoint { get; set; }

		public int Grade { get; set; }

		public int FormulaTypeId { get; set; }

		public string FormulaType { get; set; }

		public string ScoreName { get; set; }

		public List<LessonCoefficientReadModel> Coefficients { get; set; }
	}
}
