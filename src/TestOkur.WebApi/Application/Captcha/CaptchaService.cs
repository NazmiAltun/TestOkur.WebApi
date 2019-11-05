namespace TestOkur.WebApi.Application.Captcha
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CaptchaService : ICaptchaService
    {
        private readonly HttpClient _captchaClient;

        public CaptchaService(HttpClient captchaClient)
        {
            _captchaClient = captchaClient;
        }

        public async Task<bool> ValidateAsync(Guid id, string code)
        {
            if (id == Guid.Empty)
            {
                return false;
            }

            var model = new
            {
                CaptchaText = code,
            };
            var response = await _captchaClient.PostAsync($"/captcha/{id}", model.ToJsonContent());

            return response.IsSuccessStatusCode;
        }
    }
}
