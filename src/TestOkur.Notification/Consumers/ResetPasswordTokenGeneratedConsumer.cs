namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.User;
	using TestOkur.Notification.Models;

	internal class ResetPasswordTokenGeneratedConsumer
		: IConsumer<IResetPasswordTokenGenerated>
	{
		private readonly NotificationManager _notificationManager;

		public ResetPasswordTokenGeneratedConsumer(NotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		public async Task Consume(ConsumeContext<IResetPasswordTokenGenerated> context)
		{
			await _notificationManager.SendEmailAsync(
				context.Message,
				Template.PasswordResetEmailUser,
				context.Message.Email);
		}
	}
}
