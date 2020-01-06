namespace TestOkur.WebApi.Application.Lesson.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using Lesson = TestOkur.Domain.Model.LessonModel.Lesson;

    public class CreateLessonCommand : CommandBase, IClearCache
    {
        public CreateLessonCommand(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public CreateLessonCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Lessons_{UserId}" };

        public string Name { get; set; }

        public Lesson ToDomainModel() => new Lesson(Name);
    }
}
