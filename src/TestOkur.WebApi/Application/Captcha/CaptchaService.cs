namespace TestOkur.WebApi.Application.Captcha
{
    using CacheManager.Core;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CaptchaService : ICaptchaService
    {
        private readonly ICacheManager<Captcha> _captchaCache;
        private readonly HttpClient _captchaClient;

        public CaptchaService(
            ICacheManager<Captcha> captchaCache,
            HttpClient captchaClient)
        {
            _captchaCache = captchaCache;
            _captchaClient = captchaClient;
        }

        public async Task<Stream> GenerateAsync(Guid id)
        {
            var response = await _captchaClient.GetAsync("captcha");
            var code = response.Headers.GetValues("Text").First();
            _captchaCache.Add(new CacheItem<Captcha>(
                $"Captcha_{id}",
                new Captcha(id, code),
                ExpirationMode.Absolute,
                TimeSpan.FromHours(1)));

            return await response.Content.ReadAsStreamAsync();
        }

        public bool Validate(Guid id, string code)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var captcha = _captchaCache.Get($"Captcha_{id}");

            return captcha?.Code == code;
        }

    }
}
