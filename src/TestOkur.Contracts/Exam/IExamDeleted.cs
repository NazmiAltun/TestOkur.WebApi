namespace TestOkur.Contracts.Exam
{
	public interface IExamDeleted : IIntegrationEvent
	{
		int ExamId { get; }
	}
}
