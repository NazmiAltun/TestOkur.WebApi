namespace TestOkur.WebApi.Application.Student
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.Student;

	public class StudentDeleted : IntegrationEvent, IStudentDeleted
	{
		public StudentDeleted(int studentId)
		{
			StudentId = studentId;
		}

		public int StudentId { get; }
	}
}
