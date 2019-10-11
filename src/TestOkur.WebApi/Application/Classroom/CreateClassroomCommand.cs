namespace TestOkur.WebApi.Application.Classroom
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;
    using Classroom = TestOkur.Domain.Model.ClassroomModel.Classroom;

    [DataContract]
    public class CreateClassroomCommand : CommandBase, IClearCache
    {
        public CreateClassroomCommand(Guid id, int grade, string name)
         : base(id)
        {
            Grade = grade;
            Name = name;
        }

        public IEnumerable<string> CacheKeys => new[] { $"Classrooms_{UserId}" };

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int Grade { get; private set; }

        public Classroom ToDomainModel()
        {
            return new Classroom(Grade, Name);
        }
    }
}
