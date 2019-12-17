namespace TestOkur.Notification.ScheduledTasks.SmsResender
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;

    public class SmsResender : ISmsResender
    {
        private readonly ISmsRepository _smsRepository;
        private readonly ISmsClient _smsClient;

        public SmsResender(ISmsRepository smsRepository, ISmsClient smsClient)
        {
            _smsRepository = smsRepository;
            _smsClient = smsClient;
        }

        public async Task TryResendAsync()
        {
            var smses = await _smsRepository.GetPendingOrFailedSmsesAsync();

            foreach (var sms in smses)
            {
                await _smsClient.SendAsync(sms);
            }
        }
    }
}