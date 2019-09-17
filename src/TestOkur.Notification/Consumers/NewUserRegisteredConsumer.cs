namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class NewUserRegisteredConsumer : IConsumer<INewUserRegistered>
    {
        private readonly INotificationFacade _notificationFacade;

        public NewUserRegisteredConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<INewUserRegistered> context)
        {
            await _notificationFacade.SendEmailAsync(
                context.Message,
                Template.AccountRegistrationEmailUser,
                context.Message.Email);
            await _notificationFacade.SendEmailToAdminsAsync(
                context.Message,
                Template.AccountRegistrationEmailAdmin);
        }
    }
}
