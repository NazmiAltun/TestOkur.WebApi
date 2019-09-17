namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class SendSmsRequestFailedConsumer : IConsumer<ISendSmsRequestFailed>
	{
        private readonly INotificationFacade _notificationFacade;

        public SendSmsRequestFailedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<ISendSmsRequestFailed> context)
		{
			await _notificationFacade.SendEmailToSystemAdminsAsync(
                context.Message,
                Template.SmsFailureEmailAdmin);
		}
	}
}
