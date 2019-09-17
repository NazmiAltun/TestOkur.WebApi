namespace TestOkur.WebApi.Application.Exam.Commands
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Exam;

    public class ExamDeleted : IntegrationEvent, IExamDeleted
	{
		public ExamDeleted(int examId)
		{
			ExamId = examId;
		}

		public int ExamId { get; }
	}
}
