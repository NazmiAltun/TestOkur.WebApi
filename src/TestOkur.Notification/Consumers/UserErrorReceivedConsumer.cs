namespace TestOkur.Notification.Consumers
{
	using MassTransit;
	using System.Threading.Tasks;
	using TestOkur.Contracts.Alert;
	using TestOkur.Notification.Models;

	internal class UserErrorReceivedConsumer : IConsumer<IUserErrorReceived>
	{
		private readonly NotificationManager _notificationManager;

		public UserErrorReceivedConsumer(NotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		public async Task Consume(ConsumeContext<IUserErrorReceived> context)
		{
			await _notificationManager.SendEmailToAdminsAsync(
				context.Message,
				Template.UserErrorAlertEmail);
		}
	}
}
