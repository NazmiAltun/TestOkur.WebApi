namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.Account;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Models;

	internal class AccountExtendedConsumer : IConsumer<IAccountExtended>
	{
        private readonly INotificationFacade _notificationFacade;

        public AccountExtendedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<IAccountExtended> context)
		{
			await _notificationFacade.SendEmailAsync(
				context.Message,
				Template.AccountExtensionEmailUser,
				context.Message.Email);
			await _notificationFacade.SendSmsAsync(
				context.Message,
				Template.AccountExtensionSmsUser,
				context.Message.Phone);
		}
	}
}
