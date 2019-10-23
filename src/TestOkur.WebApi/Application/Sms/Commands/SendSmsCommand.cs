namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
    public class SendSmsCommand : CommandBase , IClearCache
    {
        public SendSmsCommand(
            Guid id,
            IEnumerable<SmsMessageModel> messages)
         : base(id)
        {
            Messages = messages;
        }
        public IEnumerable<string> CacheKeys => new[] { "Users" };

        [DataMember]
        public IEnumerable<SmsMessageModel> Messages { get; private set; }
    }
}
