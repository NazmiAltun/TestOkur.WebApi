namespace TestOkur.WebApi.Application.Lesson.Commands
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.Lesson;

	public class LessonNameChanged : IntegrationEvent, ILessonNameChanged
	{
		public LessonNameChanged(int lessonId, string newLessonName)
		{
			LessonId = lessonId;
			NewLessonName = newLessonName;
		}

		public int LessonId { get; }

		public string NewLessonName { get; }
	}
}
