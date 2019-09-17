namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class UserActivatedConsumer : IConsumer<IUserActivated>
    {
        private readonly INotificationFacade _notificationFacade;

        public UserActivatedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<IUserActivated> context)
        {
            await _notificationFacade.SendEmailAsync(
                context.Message,
                Template.AccountActivationEmailUser,
                context.Message.Email);
            await _notificationFacade.SendSmsAsync(
                context.Message,
                Template.AccountActivationSmsUser,
                context.Message.Phone);
        }
    }
}
