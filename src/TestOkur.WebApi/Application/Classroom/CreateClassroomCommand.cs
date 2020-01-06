namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    public class CreateClassroomCommand : CommandBase, IClearCache
    {
        public CreateClassroomCommand(Guid id, int grade, string name)
         : base(id)
        {
            Grade = grade;
            Name = name;
        }

        public CreateClassroomCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Classrooms_{UserId}" };

        public string Name { get; set; }

        public int Grade { get; set; }

        public Classroom ToDomainModel()
        {
            return new Classroom(Grade, Name);
        }
    }
}
