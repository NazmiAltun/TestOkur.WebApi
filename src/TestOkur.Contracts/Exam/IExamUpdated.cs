namespace TestOkur.Contracts.Exam
{
	using System;
	using System.Collections.Generic;
	using TestOkur.Optic.Form;

	public interface IExamUpdated : IIntegrationEvent
	{
		int ExamId { get; }

		int IncorrectEliminationRate { get; }

		DateTime ExamDate { get; }

		string ExamName { get; }

		IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; }
	}
}
