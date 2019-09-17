namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Alert;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class UserErrorReceivedConsumer : IConsumer<IUserErrorReceived>
	{
		private readonly INotificationFacade _notificationFacade;

		public UserErrorReceivedConsumer(INotificationFacade notificationFacade)
		{
			_notificationFacade = notificationFacade;
		}

		public async Task Consume(ConsumeContext<IUserErrorReceived> context)
		{
			await _notificationFacade.SendEmailToAdminsAsync(
				context.Message,
				Template.UserErrorAlertEmail);
		}
	}
}
