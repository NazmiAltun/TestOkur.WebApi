namespace TestOkur.WebApi.Application.User.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class DeleteUserCommand : CommandBase, IClearCache
    {
        public DeleteUserCommand(int deleteUserId)
        {
            DeleteUserId = deleteUserId;
        }

        public DeleteUserCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };

        public int DeleteUserId { get; }
    }
}
