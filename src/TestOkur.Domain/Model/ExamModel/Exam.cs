namespace TestOkur.Domain.Model.ExamModel
{
	using System;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.SeedWork;

	public class Exam : Entity, IAuditable
	{
		public Exam(
			Name name,
			DateTime examDate,
			ExamType examType,
			ExamBookletType examBookletType,
			IncorrectEliminationRate incorrectEliminationRate,
			AnswerFormFormat answerFormFormat,
			Lesson lesson,
			string applicableFormTypeCode,
			string notes)
		 : this()
		{
			AnswerFormFormat = answerFormFormat;
			ExamBookletType = examBookletType;
			Name = name;
			ExamDate = examDate;
			ExamType = examType;
			IncorrectEliminationRate = incorrectEliminationRate;
			Notes = notes;
			ApplicableFormTypeCode = applicableFormTypeCode;
			Lesson = lesson;
		}

		protected Exam()
		{
		}

		public AnswerFormFormat AnswerFormFormat { get; private set; }

		public Name Name { get; private set; }

		public ExamType ExamType { get; private set; }

		public IncorrectEliminationRate IncorrectEliminationRate { get; private set; }

		public string Notes { get; private set; }

		public DateTime ExamDate { get; private set; }

		public string ApplicableFormTypeCode { get; private set; }

		public ExamBookletType ExamBookletType { get; private set; }

		public Lesson Lesson { get; private set; }

		public void Update(
			string newName,
			ExamType examType,
			int incorrectEliminationRate,
			DateTime examDate,
			string applicableFormTypeCode,
			AnswerFormFormat newAnswerFormFormat,
			Lesson lesson,
			ExamBookletType examBookletType,
			string newNotes)
		{
			Name = newName;
			AnswerFormFormat = newAnswerFormFormat;
			ExamType = examType;
			IncorrectEliminationRate = incorrectEliminationRate;
			ExamDate = examDate;
			ApplicableFormTypeCode = applicableFormTypeCode;
			Notes = newNotes;
			Lesson = lesson;
			ExamBookletType = examBookletType;
		}
	}
}
