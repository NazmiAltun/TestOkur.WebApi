namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Notification.Configuration;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Models;

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
            var request = CreateRequest(sms, sms.Body);
            var response = await _httpClient.SendAsync(request);
            await EnsureSuccess(response);

            return sms.Body;
        }

        private HttpRequestMessage CreateRequest(Sms sms, string smsFriendlyBody)
        {
            var values = new Dictionary<string, string>
            {
                { "kullanici", _smsConfiguration.User },
                { "sifre", _smsConfiguration.Password },
                { "gonderenadi", sms.Subject },
                { "mesaj", smsFriendlyBody },
                { "numaralar", sms.Phone },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _smsConfiguration.ServiceUrl)
            {
                Content = new FormUrlEncodedContent(values),
            };
            request.Properties.Add("sms", sms);
            return request;
        }

        private async Task EnsureSuccess(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadAsStringAsync();

            if (message.Contains("|"))
            {
                throw new SmsException(message.Substring(message.LastIndexOf('|') + 1));
            }
        }
    }
}
