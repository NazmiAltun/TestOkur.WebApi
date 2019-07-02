namespace TestOkur.Contracts.Student
{
	public interface IStudentDeleted : IIntegrationEvent
	{
		int StudentId { get; }
	}
}
