namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class BulkCreateStudentCommand : CommandBase, IClearCache
    {
        public BulkCreateStudentCommand(Guid id, IEnumerable<CreateStudentCommand> commands)
         : base(id)
        {
            Commands = commands;
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        [DataMember]
        public IEnumerable<CreateStudentCommand> Commands { get; private set; }
    }
}
