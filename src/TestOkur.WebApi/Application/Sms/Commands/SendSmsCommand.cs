namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class SendSmsCommand : CommandBase, IClearCache
    {
        public SendSmsCommand(
            Guid id,
            IEnumerable<SmsMessageModel> messages)
         : base(id)
        {
            Messages = messages;
        }

        public SendSmsCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { "Users" };

        public IEnumerable<SmsMessageModel> Messages { get; set; }
    }
}
