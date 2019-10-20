namespace TestOkur.Notification.ScheduledTasks.SmsResender
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;

    public class SmsResender : ISmsResender
    {
        private readonly ISmsRepository _smsRepository;
        private readonly ISmsClient _smsClient;
        private readonly IWebApiClient _webApiClient;

        public SmsResender(ISmsRepository smsRepository, ISmsClient smsClient, IWebApiClient webApiClient)
        {
            _smsRepository = smsRepository;
            _smsClient = smsClient;
            _webApiClient = webApiClient;
        }

        public async Task TryResendAsync()
        {
            var smses = await _smsRepository.GetPendingOrFailedSmsesTodayAsync();

            foreach (var sms in smses)
            {
                var smsBody = await _smsClient.SendAsync(sms);

                if (sms.UserId != default)
                {
                    await _webApiClient.DeductSmsCreditsAsync(sms.UserId, smsBody);
                }
            }
        }
    }
}