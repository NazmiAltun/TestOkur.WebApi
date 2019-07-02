namespace TestOkur.Notification.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using TestOkur.Contracts.User;
	using TestOkur.Notification.Models;

	internal class NewUserRegisteredConsumer : IConsumer<INewUserRegistered>
	{
        private readonly NotificationManager _notificationManager;

        public NewUserRegisteredConsumer(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public async Task Consume(ConsumeContext<INewUserRegistered> context)
		{
			await _notificationManager.SendEmailAsync(
				context.Message,
				Template.AccountRegistrationEmailUser,
				context.Message.Email);
			await _notificationManager.SendEmailToAdminsAsync(
                context.Message,
                Template.AccountRegistrationEmailAdmin);
		}
	}
}
