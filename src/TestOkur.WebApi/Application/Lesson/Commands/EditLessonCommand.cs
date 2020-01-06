namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class EditLessonCommand : CommandBase, IClearCache
    {
        public EditLessonCommand(Guid id, int lessonId, string newName)
            : base(id)
        {
            LessonId = lessonId;
            NewName = newName;
        }

        public EditLessonCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Lessons_{UserId}" };

        public int LessonId { get; set; }

        public string NewName { get; set; }
    }
}
