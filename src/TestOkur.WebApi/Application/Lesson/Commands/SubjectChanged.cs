namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Lesson;

    public class SubjectChanged : IntegrationEvent, ISubjectChanged
	{
		public SubjectChanged(int subjectId, string newName)
		{
			SubjectId = subjectId;
			NewName = newName;
		}

		public int SubjectId { get; }

		public string NewName { get; }
	}
}
