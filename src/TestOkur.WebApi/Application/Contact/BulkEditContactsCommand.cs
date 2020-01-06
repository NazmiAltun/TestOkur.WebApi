namespace TestOkur.WebApi.Application.Contact
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class BulkEditContactsCommand : CommandBase, IClearCache
    {
        public BulkEditContactsCommand(Guid id, IEnumerable<EditContactCommand> commands)
            : base(id)
        {
            Commands = commands;
        }

        public BulkEditContactsCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };

        public IEnumerable<EditContactCommand> Commands { get; set; }
    }
}
