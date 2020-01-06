namespace TestOkur.WebApi.Application.User.Commands
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class ActivateUserCommand : CommandBase, IClearCache
    {
        public ActivateUserCommand(string email)
        {
            Email = email;
        }

        public ActivateUserCommand()
        {
        }

        public string Email { get; set; }

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };
    }
}
