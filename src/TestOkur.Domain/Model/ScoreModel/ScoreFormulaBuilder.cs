namespace TestOkur.Domain.Model.ScoreModel
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Domain.Model.OpticalFormModel;

    public class ScoreFormulaBuilder
	{
		private readonly List<LessonCoefficient> _lessonCoefficients;
		private readonly List<Grade> _grades;

		private float _basePoint;
		private string _name;
		private FormulaType _formulaType;

		public ScoreFormulaBuilder()
		{
			_lessonCoefficients = new List<LessonCoefficient>();
			_grades = new List<Grade>();
		}

		public ScoreFormulaBuilder WithName(string name)
		{
			_name = name;
			return this;
		}

		public ScoreFormulaBuilder WithGrade(Grade value)
		{
			_grades.Add(value);
			return this;
		}

		public ScoreFormulaBuilder SecondarySchool()
		{
			_grades.AddRange(new Grade[] { 5, 6, 7, 8 });
			return this;
		}

		public ScoreFormulaBuilder HighSchool()
		{
			_grades.Add(9);
			return this;
		}

		public ScoreFormulaBuilder WithBasePoint(float value)
		{
			_basePoint = value;
			return this;
		}

		public ScoreFormulaBuilder WithFormulaType(FormulaType value)
		{
			_formulaType = value;
			return this;
		}

		public ScoreFormulaBuilder AddLessonCoefficient(
			OpticalFormType opticalFormType,
			string lessonName,
			float coefficient)
		{
			var section = opticalFormType.FormLessonSections
				.First(e => e.Lesson.Name == lessonName);

			_lessonCoefficients.Add(
				new LessonCoefficient(section, coefficient));

			return this;
		}

		public IEnumerable<ScoreFormula> Build()
		{
			var list = new List<ScoreFormula>();

			foreach (var grade in _grades)
			{
				list.Add(new ScoreFormula(
					_name ?? $"{grade}. Sınıf",
					grade.Value,
					_basePoint,
					_formulaType,
					_lessonCoefficients.Select(
						lc => new LessonCoefficient(lc.ExamLessonSection, lc.Coefficient))
						.ToList()));
			}

			return list;
		}
	}
}
