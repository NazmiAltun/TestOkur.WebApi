namespace TestOkur.WebApi.Application.Student
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class BulkCreateStudentCommand : CommandBase, IClearCache
    {
        public BulkCreateStudentCommand(Guid id, IEnumerable<CreateStudentCommand> commands)
         : base(id)
        {
            Commands = commands;
        }

        public BulkCreateStudentCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public IEnumerable<CreateStudentCommand> Commands { get; set; }
    }
}
