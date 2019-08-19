namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.User;
	using TestOkur.Notification.Models;

	internal class UserActivatedConsumer : IConsumer<IUserActivated>
	{
        private readonly NotificationManager _notificationManager;

        public UserActivatedConsumer(NotificationManager notificationManager)
		{
            _notificationManager = notificationManager;
        }

        public async Task Consume(ConsumeContext<IUserActivated> context)
		{
			await _notificationManager.SendEmailAsync(
				context.Message,
				Template.AccountActivationEmailUser,
				context.Message.Phone);
			await _notificationManager.SendSmsAsync(
				context.Message,
				Template.LicenseExpirationNoticeSms,
				context.Message.Email);
		}
	}
}
