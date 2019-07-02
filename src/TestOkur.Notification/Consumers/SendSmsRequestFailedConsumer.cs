namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Sms;
	using TestOkur.Notification.Models;

	internal class SendSmsRequestFailedConsumer : IConsumer<ISendSmsRequestFailed>
	{
        private readonly NotificationManager _notificationManager;

        public SendSmsRequestFailedConsumer(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async Task Consume(ConsumeContext<ISendSmsRequestFailed> context)
		{
			await _notificationManager.SendEmailToSystemAdminsAsync(
                context.Message,
                Template.SmsFailureEmailAdmin);
		}
	}
}
