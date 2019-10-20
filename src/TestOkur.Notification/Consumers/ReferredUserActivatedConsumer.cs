namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using System.Threading.Tasks;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Models;

    internal class ReferredUserActivatedConsumer : IConsumer<IReferredUserActivated>
    {
        private readonly INotificationFacade _notificationFacade;

        public ReferredUserActivatedConsumer(INotificationFacade notificationFacade)
        {
            _notificationFacade = notificationFacade;
        }

        public async Task Consume(ConsumeContext<IReferredUserActivated> context)
        {
            await _notificationFacade.SendSmsAsync(
                context.Message,
                Template.RefereeSmsCreditsAddedSms,
                context.Message.RefereePhone);
            await _notificationFacade.SendSmsAsync(
                context.Message,
                Template.ReferrerSmsCreditsAddedSms,
                context.Message.ReferrerPhone);
        }
    }
}
