namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Paramore.Brighter;
	using TestOkur.Contracts.Sms;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class SendSmsAdminCommandHandler : RequestHandlerAsync<SendSmsAdminCommand>
	{
		private const string Subject = "TESTOKUR";

		private readonly IPublishEndpoint _publishEndpoint;

		public SendSmsAdminCommandHandler(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint ??
							   throw new ArgumentNullException(nameof(publishEndpoint));
		}

		[Idempotent(1)]
		[Populate(2)]
		public override async Task<SendSmsAdminCommand> HandleAsync(
			SendSmsAdminCommand command,
			CancellationToken cancellationToken = default)
		{
			var @event = new SendSmsRequestReceived(
				default,
				default,
				new[] { new SmsMessage(command.Receiver, Subject, command.Body, 0), },
				"nazmialtun@windowslive.com");
			await _publishEndpoint.Publish<ISendSmsRequestReceived>(
				@event,
				cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
