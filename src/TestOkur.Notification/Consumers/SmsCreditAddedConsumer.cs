namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class SmsCreditAddedConsumer : IConsumer<ISmsCreditAdded>
	{
        private readonly INotificationFacade _notificationFacade;

        public SmsCreditAddedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<ISmsCreditAdded> context)
		{
			await _notificationFacade.SendEmailAsync(
				context.Message,
				Template.SmsCreditAddedEmailUser,
				context.Message.Email);
			await _notificationFacade.SendSmsAsync(
				context.Message,
				Template.SmsCreditAddedSmsUser,
				context.Message.Phone);
		}
	}
}
