namespace TestOkur.Domain.Model.ScoreModel
{
	using System.Collections.Generic;
	using TestOkur.Domain.Model.ExamModel;

	public class ExamScoreFormula : ScoreFormula
	{
		public ExamScoreFormula(
			Exam exam,
			Name name,
			Grade grade,
			float basePoint,
			FormulaType formulaType,
			List<LessonCoefficient> coefficients)
		 : base(name, grade, basePoint, formulaType, coefficients)
		{
			Exam = exam;
		}

		protected ExamScoreFormula()
		{
		}

		public Exam Exam { get; private set; }
	}
}