namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class SendSmsAdminCommand : CommandBase
    {
        public SendSmsAdminCommand(Guid id, string receiver, string body)
            : base(id)
        {
            Receiver = receiver;
            Body = body;
        }

        [DataMember]
        public string Body { get; private set; }

        [DataMember]
        public string Receiver { get; private set; }
    }
}
