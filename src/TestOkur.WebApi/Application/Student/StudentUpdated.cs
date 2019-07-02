namespace TestOkur.WebApi.Application.Student
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.Student;

	public class StudentUpdated : IntegrationEvent, IStudentUpdated
	{
		public StudentUpdated(int classroomId, int studentNumber, string lastName, string firstName, int studentId)
		{
			ClassroomId = classroomId;
			StudentNumber = studentNumber;
			LastName = lastName;
			FirstName = firstName;
			StudentId = studentId;
		}

		public int StudentId { get; }

		public string FirstName { get; }

		public string LastName { get; }

		public int StudentNumber { get; }

		public int ClassroomId { get; }
	}
}
