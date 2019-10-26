namespace TestOkur.Notification.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using TestOkur.Contracts.User;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class UserActivatedConsumer : IConsumer<IUserActivated>
    {
        private readonly INotificationFacade _notificationFacade;
        private readonly ISmsLogRepository _smsLogRepository;

        public UserActivatedConsumer(INotificationFacade notificationFacade, ISmsLogRepository smsLogRepository)
        {
            _notificationFacade = notificationFacade;
            _smsLogRepository = smsLogRepository;
        }

        public async Task Consume(ConsumeContext<IUserActivated> context)
        {
            await _smsLogRepository.LogAsync(SmsLog.CreateInitialLog(context.Message));
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
