namespace TestOkur.WebApi.Application.Classroom
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Classroom;

    public class ClassroomDeleted : IntegrationEvent, IClassroomDeleted
	{
		public ClassroomDeleted(int classroomId)
		{
			ClassroomId = classroomId;
		}

		public int ClassroomId { get; }
	}
}
