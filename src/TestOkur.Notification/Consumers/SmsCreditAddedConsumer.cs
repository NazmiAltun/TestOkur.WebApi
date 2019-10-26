namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class SmsCreditAddedConsumer : IConsumer<ISmsCreditAdded>
    {
        private readonly INotificationFacade _notificationFacade;
        private readonly ISmsLogRepository _smsLogRepository;

        public SmsCreditAddedConsumer(INotificationFacade notificationFacade, ISmsLogRepository smsLogRepository)
        {
            _notificationFacade = notificationFacade;
            _smsLogRepository = smsLogRepository;
        }

        public async Task Consume(ConsumeContext<ISmsCreditAdded> context)
        {
            await _smsLogRepository.LogAsync(SmsLog.CreateSmsAdditionLog(context.Message));
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
