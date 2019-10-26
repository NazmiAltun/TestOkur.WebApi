namespace TestOkur.Notification.Consumers
{
    using MassTransit;
    using System.Threading.Tasks;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class ReferredUserActivatedConsumer : IConsumer<IReferredUserActivated>
    {
        private readonly INotificationFacade _notificationFacade;
        private readonly ISmsLogRepository _smsLogRepository;

        public ReferredUserActivatedConsumer(INotificationFacade notificationFacade, ISmsLogRepository smsLogRepository)
        {
            _notificationFacade = notificationFacade;
            _smsLogRepository = smsLogRepository;
        }

        public async Task Consume(ConsumeContext<IReferredUserActivated> context)
        {
            await _smsLogRepository.LogAsync(SmsLog.CreateSmsPromotionLogs(context.Message));

            await _notificationFacade.SendEmailAsync(
                context.Message,
                Template.RefereeSmsCreditsAddedEmail,
                context.Message.RefereeEmail);
            await _notificationFacade.SendEmailAsync(
                context.Message,
                Template.ReferrerSmsCreditsAddedEmail,
                context.Message.ReferrerEmail);

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
