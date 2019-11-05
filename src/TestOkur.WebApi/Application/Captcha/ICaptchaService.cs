namespace TestOkur.WebApi.Application.Captcha
{
    using System;
    using System.Threading.Tasks;

    public interface ICaptchaService
    {
        Task<bool> ValidateAsync(Guid id, string code);
    }
}
