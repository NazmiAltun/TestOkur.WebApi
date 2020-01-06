namespace TestOkur.WebApi.Application.User.Commands
{
    using TestOkur.Infrastructure.CommandsQueries;

    public class UpdateUserOnlineStatusCommand : CommandBase, ISkipLogging
    {
        public UpdateUserOnlineStatusCommand(string email)
        {
            Email = email;
        }

        public UpdateUserOnlineStatusCommand()
        {
        }

        public string Email { get; set; }
    }
}
