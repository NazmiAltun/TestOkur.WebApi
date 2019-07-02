namespace TestOkur.Domain.Model.OpticalFormModel
{
	using System;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.SeedWork;

	public class FormLessonSection : Entity
	{
		public FormLessonSection(Lesson lesson, int maxQuestionCount, int formPart)
			: this(lesson, maxQuestionCount, default, formPart)
		{
		}

		public FormLessonSection(Lesson lesson, int maxQuestionCount)
		 : this(lesson, maxQuestionCount, default, default)
		{
		}

		public FormLessonSection(Lesson lesson, int maxQuestionCount, string nameTag)
		 : this(lesson, maxQuestionCount, nameTag, default)
		{
		}

		public FormLessonSection(Lesson lesson, int maxQuestionCount, string nameTag, int formPart)
		{
			if (maxQuestionCount <= 0 ||
				maxQuestionCount > 120)
			{
				throw new ArgumentOutOfRangeException(
					nameof(maxQuestionCount),
					$"Invalid MaxQuestionCount Value : {maxQuestionCount}");
			}

			FormPart = formPart;
			Lesson = lesson;
			MaxQuestionCount = maxQuestionCount;
			NameTag = nameTag;
		}

		protected FormLessonSection()
		{
		}

		public Lesson Lesson { get; private set; }

		public int MaxQuestionCount { get; private set; }

		public string NameTag { get; private set; }

		public int FormPart { get; private set; }
	}
}
