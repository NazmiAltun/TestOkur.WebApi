namespace TestOkur.Notification.ScheduledTasks.SmsResender
{
    using System.Linq;
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
            var tasks = smses.Select(sms => _smsClient.SendAsync(sms));
            await Task.WhenAll(tasks);
        }
    }
}