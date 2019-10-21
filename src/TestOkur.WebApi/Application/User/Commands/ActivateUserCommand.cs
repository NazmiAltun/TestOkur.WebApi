namespace TestOkur.WebApi.Application.User.Commands
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class ActivateUserCommand : CommandBase, IClearCache
    {
        public ActivateUserCommand(string email)
        {
            Email = email;
        }

        [DataMember]
        public string Email { get; }

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };
    }
}
