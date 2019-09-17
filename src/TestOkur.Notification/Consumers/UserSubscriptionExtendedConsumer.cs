namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class UserSubscriptionExtendedConsumer : IConsumer<IUserSubscriptionExtended>
	{
        private readonly INotificationFacade _notificationFacade;

        public UserSubscriptionExtendedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<IUserSubscriptionExtended> context)
		{
			await _notificationFacade.SendEmailAsync(
				context.Message,
				Template.UserSubscriptionExtendedEmail,
				context.Message.Email);
			await _notificationFacade.SendSmsAsync(
				context.Message,
				Template.UserSubscriptionExtendedSms,
				context.Message.Phone);
		}
	}
}
