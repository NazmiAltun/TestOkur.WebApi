namespace TestOkur.WebApi.Application.User.Commands
{
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class ActivateUserCommand : CommandBase
    {
        public ActivateUserCommand(string email)
        {
            Email = email;
        }

        [DataMember]
        public string Email { get; }
    }
}
