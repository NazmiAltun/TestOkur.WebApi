namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public class CreateUnitCommand : CommandBase, IClearCache
    {
        public CreateUnitCommand(Guid id, string name, int lessonId, int grade)
            : base(id)
        {
            Name = name;
            LessonId = lessonId;
            Grade = grade;
        }

        public CreateUnitCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Units_{UserId}" };

        public string Name { get; set; }

        public int Grade { get; set; }

        public int LessonId { get; set; }

        public Unit ToDomainModel(Lesson lesson) => new Unit(Name, lesson, Grade, false);
    }
}
