namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Dtos;

    public class SmsClient : ISmsClient
    {
        private readonly HttpClient _httpClient;
        private readonly SmsConfiguration _smsConfiguration;

        public SmsClient(HttpClient httpClient, SmsConfiguration smsConfiguration)
        {
            _httpClient = httpClient;
            _smsConfiguration = smsConfiguration;
        }

        public async Task<string> SendAsync(Sms sms)
        {
            var subject = MapSubject(sms.Subject);
            var url =
                $"{_smsConfiguration.ServiceUrl}?kno={_smsConfiguration.UserId}&kul_ad={_smsConfiguration.User}&sifre={_smsConfiguration.Password}" +
                $"&gonderen={subject}&mesaj={sms.Body}&numaralar={sms.Phone}&tur=Normal";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static string MapSubject(string subject)
        {
            if (subject == "VELI BILGI" || subject == "OKUL BILGI" || subject == "OKUL DUYURU")
            {
                return "VELIBILG.NF";
            }

            return "SINAVBIL.NF";
        }
    }
}
