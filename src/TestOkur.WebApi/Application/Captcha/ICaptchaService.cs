namespace TestOkur.WebApi.Application.Captcha
{
	using System;
	using System.IO;

	public interface ICaptchaService
    {
        Stream Generate(Guid id);

        bool Validate(Guid id, string code);
    }
}
