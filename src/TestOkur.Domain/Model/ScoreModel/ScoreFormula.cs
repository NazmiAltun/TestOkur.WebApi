namespace TestOkur.Domain.Model.ScoreModel
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.SeedWork;

    public class ScoreFormula : Entity, IAuditable
	{
		private readonly List<LessonCoefficient> _coefficients
			= new List<LessonCoefficient>();

		public ScoreFormula(ScoreFormula formula)
		{
			Grade = formula.Grade.Value;
			Name = formula.Name.Value;
			BasePoint = formula.BasePoint;
			FormulaType = formula.FormulaType;

			foreach (var coef in formula.Coefficients)
			{
				_coefficients.Add(new LessonCoefficient(
					coef.ExamLessonSection,
					coef.Coefficient));
			}
		}

		public ScoreFormula(
			Name name,
			Grade grade,
			float basePoint,
			FormulaType formulaType,
			List<LessonCoefficient> coefficients)
		{
			Grade = grade;
			Name = name;
			BasePoint = basePoint;
			FormulaType = formulaType;
			_coefficients = coefficients;
		}

		protected ScoreFormula()
		{
		}

		public Name Name { get; private set; }

		public Grade Grade { get; private set; }

		public float BasePoint { get; private set; }

		public FormulaType FormulaType { get; private set; }

		public IEnumerable<LessonCoefficient> Coefficients =>
			_coefficients.AsReadOnly();

		public void Update(float basePoint, Dictionary<int, float> coefficients)
		{
			BasePoint = basePoint;

			foreach (var id in coefficients.Keys)
			{
				_coefficients.First(c => c.Id == id)
					.SetNewCoefficient(coefficients[id]);
			}
		}
	}
}
