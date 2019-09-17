namespace TestOkur.Domain.Model.ScoreModel
{
    using TestOkur.Domain.Model.OpticalFormModel;
    using TestOkur.Domain.SeedWork;

    public class LessonCoefficient : Entity
	{
		public LessonCoefficient(
			FormLessonSection examLessonSection,
			float coefficient)
		{
			ExamLessonSection = examLessonSection;
			Coefficient = coefficient;
		}

		protected LessonCoefficient()
		{
		}

		public FormLessonSection ExamLessonSection { get; private set; }

		public float Coefficient { get; private set; }

		public void SetNewCoefficient(float coefficient)
		{
			Coefficient = coefficient;
		}
	}
}
