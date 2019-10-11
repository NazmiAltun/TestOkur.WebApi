namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    [DataContract]
    public class CreateLessonCommand : CommandBase, IClearCache
    {
        public CreateLessonCommand(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Lessons_{UserId}" };

        [DataMember]
        public string Name { get; private set; }

        public Lesson ToDomainModel() => new Lesson(Name);
    }
}
