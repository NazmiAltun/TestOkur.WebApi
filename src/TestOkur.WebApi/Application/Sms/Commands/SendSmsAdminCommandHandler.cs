namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Paramore.Brighter;
    using TestOkur.Contracts.Sms;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class SendSmsAdminCommandHandler : RequestHandlerAsync<SendSmsAdminCommand>
    {
        private const string Subject = "TESTOKUR";

        private readonly IBus _bus;

        public SendSmsAdminCommandHandler(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        [Idempotent(1)]
        public override async Task<SendSmsAdminCommand> HandleAsync(
            SendSmsAdminCommand command,
            CancellationToken cancellationToken = default)
        {
            var @event = new SendSmsRequestReceived(
                default,
                default,
                new[] { new SmsMessage(command.Receiver, Subject, command.Body, 0), },
                "nazmialtun@windowslive.com");
            await _bus.Publish<ISendSmsRequestReceived>(
                @event,
                cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
