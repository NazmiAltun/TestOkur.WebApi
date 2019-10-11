namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public sealed class EditLessonCommand : CommandBase, IClearCache
    {
        public EditLessonCommand(Guid id, int lessonId, string newName)
            : base(id)
        {
            LessonId = lessonId;
            NewName = newName;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Lessons_{UserId}" };

        [DataMember]
        public int LessonId { get; private set; }

        [DataMember]
        public string NewName { get; private set; }
    }
}
