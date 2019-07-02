namespace TestOkur.WebApi.Application.Exam.Commands
{
	using System;
	using System.Collections.Generic;
	using TestOkur.Contracts;
	using TestOkur.Contracts.Exam;
	using TestOkur.Optic.Form;
	using Exam = TestOkur.Domain.Model.ExamModel.Exam;

	public class ExamCreated : IntegrationEvent, IExamCreated
	{
		public ExamCreated(Exam exam, IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms)
		{
			ExamId = (int)exam.Id;
			IncorrectEliminationRate = exam.IncorrectEliminationRate;
			ExamDate = exam.ExamDate;
			ExamName = exam.Name.Value;
			AnswerKeyOpticalForms = answerKeyOpticalForms;
		}

		public int ExamId { get; }

		public int IncorrectEliminationRate { get; }

		public DateTime ExamDate { get; }

		public string ExamName { get; }

		public IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; }
	}
}
