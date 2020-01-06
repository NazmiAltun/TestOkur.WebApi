namespace TestOkur.WebApi.Application.Contact
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteContactCommand : CommandBase, IClearCache
    {
        public DeleteContactCommand(int contactId)
        {
            ContactId = contactId;
        }

        public DeleteContactCommand()
        {
        }

        public int ContactId { get; }

        public IEnumerable<string> CacheKeys => new[]
        {
            $"Students_{UserId}",
            $"Contacts_{UserId}",
        };
    }
}
