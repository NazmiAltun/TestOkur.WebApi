namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.Cqrs;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    [DataContract]
    public class CreateUnitCommand : CommandBase, IClearCache
    {
        public CreateUnitCommand(Guid id, string name, int lessonId, int grade)
            : base(id)
        {
            Name = name;
            LessonId = lessonId;
            Grade = grade;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int Grade { get; private set; }

        [DataMember]
        public int LessonId { get; private set; }

        public Unit ToDomainModel(Lesson lesson) => new Unit(Name, lesson, Grade);
    }
}
