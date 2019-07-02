namespace TestOkur.Contracts.Lesson
{
	public interface ISubjectChanged : IIntegrationEvent
	{
		int SubjectId { get; }

		string NewName { get; }
	}
}
