namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Account;
	using TestOkur.Notification.Models;

	internal class SmsCreditAddedConsumer : IConsumer<ISmsCreditAdded>
	{
        private readonly NotificationManager _notificationManager;

        public SmsCreditAddedConsumer(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async Task Consume(ConsumeContext<ISmsCreditAdded> context)
		{
			await _notificationManager.SendEmailAsync(
				context.Message,
				Template.SmsCreditAddedEmailUser,
				context.Message.Phone);
			await _notificationManager.SendSmsAsync(
				context.Message,
				Template.SmsCreditAddedSmsUser,
				context.Message.Email);
		}
	}
}
