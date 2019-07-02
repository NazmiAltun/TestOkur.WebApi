namespace TestOkur.WebApi.Application.Captcha
{
	using System;

	public class Captcha
    {
        public Captcha()
        {
        }

        public Captcha(Guid id, string code)
        {
            Id = id;
            Code = code;
        }

        public Guid Id { get; set; }

        public string Code { get; set; }
    }
}
