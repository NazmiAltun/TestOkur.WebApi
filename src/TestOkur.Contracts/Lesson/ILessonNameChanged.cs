namespace TestOkur.Contracts.Lesson
{
    public interface ILessonNameChanged : IIntegrationEvent
    {
        int LessonId { get; }

        string NewLessonName { get; }
    }
}
