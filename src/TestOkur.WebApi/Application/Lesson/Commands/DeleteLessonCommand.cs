namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteLessonCommand : CommandBase, IClearCache
    {
        public DeleteLessonCommand(int lessonId)
        {
            LessonId = lessonId;
        }

        public DeleteLessonCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Lessons_{UserId}" };

        public int LessonId { get; set; }
    }
}
