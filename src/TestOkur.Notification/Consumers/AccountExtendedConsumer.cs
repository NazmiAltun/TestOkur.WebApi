namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Account;
	using TestOkur.Notification.Models;

	internal class AccountExtendedConsumer : IConsumer<IAccountExtended>
	{
        private readonly NotificationManager _notificationManager;

        public AccountExtendedConsumer(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async Task Consume(ConsumeContext<IAccountExtended> context)
		{
			await _notificationManager.SendEmailAsync(
				context.Message,
				Template.AccountExtensionEmailUser,
				context.Message.Email);
			await _notificationManager.SendSmsAsync(
				context.Message,
				Template.AccountExtensionSmsUser,
				context.Message.Phone);
		}
	}
}
