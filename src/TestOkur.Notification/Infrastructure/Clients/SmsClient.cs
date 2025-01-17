using System;

namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DnsClient.Internal;
    using Microsoft.Extensions.Logging;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Dtos;
    using TestOkur.Notification.Infrastructure.Data;

    public class SmsClient : ISmsClient
    {
        private readonly HttpClient _httpClient;
        private readonly SmsConfiguration _smsConfiguration;
        private readonly ILogger<SmsClient> _logger;
        private readonly ISmsRepository _smsRepository;

        public SmsClient(HttpClient httpClient, SmsConfiguration smsConfiguration, ILogger<SmsClient> logger, ISmsRepository smsRepository)
        {
            _httpClient = httpClient;
            _smsConfiguration = smsConfiguration;
            _logger = logger;
            _smsRepository = smsRepository;
        }

        public async Task<string> SendAsync(Sms sms)
        {
            var subject = MapSubject(sms.Subject);

            var url =
                $"{_smsConfiguration.ServiceUrl}?kno={_smsConfiguration.UserId}&kul_ad={_smsConfiguration.User}&sifre={_smsConfiguration.Password}" +
                $"&gonderen={subject}&mesaj={sms.Body}&numaralar={sms.Phone}&tur=Normal";

            _logger.LogWarning("Sending SMS: {url}", url);
            var requestDateTimeUtc = DateTime.UtcNow;
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            _logger.LogWarning("SMS sent: {result}", result);
            sms.ServiceRequest = url;
            sms.ServiceResponse = result;
            sms.RequestDateTimeUtc = requestDateTimeUtc;
            sms.ResponseDateTimeUtc = DateTime.UtcNow;
            sms.Status = SmsStatus.Successful;

            await _smsRepository.UpdateSmsAsync(sms);

            return result;
        }

        private static string MapSubject(string subject)
        {
            if (subject == "SINAV BILGI" || subject == "SINAV SONUC")
            {
                return "SINAVBIL.NF";
            }

            if (subject.ToUpperInvariant() == "TESTOKUR")
            {
                return "NF.BILGI.";
            }

            return "VELIBILG.NF";
        }
    }
}
