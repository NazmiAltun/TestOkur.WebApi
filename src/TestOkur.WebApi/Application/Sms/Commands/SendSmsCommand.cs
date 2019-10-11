namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class SendSmsCommand : CommandBase
    {
        public SendSmsCommand(
            Guid id,
            IEnumerable<SmsMessageModel> messages)
         : base(id)
        {
            Messages = messages;
        }

        [DataMember]
        public IEnumerable<SmsMessageModel> Messages { get; private set; }
    }
}
