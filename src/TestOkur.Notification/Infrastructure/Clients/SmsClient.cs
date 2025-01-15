namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DnsClient.Internal;
    using Microsoft.Extensions.Logging;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Dtos;

    public class SmsClient : ISmsClient
    {
        private readonly HttpClient _httpClient;
        private readonly SmsConfiguration _smsConfiguration;
        private readonly ILogger<SmsClient> _logger;

        public SmsClient(HttpClient httpClient, SmsConfiguration smsConfiguration, ILogger<SmsClient> logger)
        {
            _httpClient = httpClient;
            _smsConfiguration = smsConfiguration;
            _logger = logger;
        }

        public async Task<string> SendAsync(Sms sms)
        {
            var subject = MapSubject(sms.Subject);

            var url =
                $"{_smsConfiguration.ServiceUrl}?kno={_smsConfiguration.UserId}&kul_ad={_smsConfiguration.User}&sifre={_smsConfiguration.Password}" +
                $"&gonderen={subject}&mesaj={sms.Body}&numaralar={sms.Phone}&tur=Normal";

            _logger.LogWarning("Sending SMS: {url}", url);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Properties.Add("sms", sms);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogWarning("SMS sent: {result}", result);

            return result;
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
