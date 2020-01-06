namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using TestOkur.Infrastructure.CommandsQueries;

    public class SendSmsAdminCommand : CommandBase
    {
        public SendSmsAdminCommand(Guid id, string receiver, string body)
            : base(id)
        {
            Receiver = receiver;
            Body = body;
        }

        public SendSmsAdminCommand()
        {
        }

        public string Body { get; set; }

        public string Receiver { get; set; }
    }
}
