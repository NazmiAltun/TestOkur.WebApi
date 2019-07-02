namespace TestOkur.WebApi.Application.Classroom
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.Classroom;

	public class ClassroomUpdated : IntegrationEvent, IClassroomUpdated
	{
		public ClassroomUpdated(int classroomId, int grade, string name)
		{
			ClassroomId = classroomId;
			Grade = grade;
			Name = name;
		}

		public int ClassroomId { get; }

		public int Grade { get; }

		public string Name { get; }
	}
}
