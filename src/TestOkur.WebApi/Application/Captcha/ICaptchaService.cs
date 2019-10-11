namespace TestOkur.WebApi.Application.Captcha
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface ICaptchaService
    {
        Task<Stream> GenerateAsync(Guid id);

        bool Validate(Guid id, string code);
    }
}
