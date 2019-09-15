namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class ExtendUserSubscriptionCommand : CommandBase
    {
        public ExtendUserSubscriptionCommand(string email,DateTime currentExpiryDateTimeUtc)
        {
            Email = email;
            CurrentExpiryDateTimeUtc = currentExpiryDateTimeUtc;
        }

        [DataMember]
        public string Email { get; private set; }

        [DataMember]
        public DateTime CurrentExpiryDateTimeUtc { get; private set; }
    }
}
