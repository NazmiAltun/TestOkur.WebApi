namespace TestOkur.WebApi.Application.Captcha
{
	using System;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using TestOkur.Common;

	[Route("api/v1/captcha")]
	[Authorize(AuthorizationPolicies.Public)]
	public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        [HttpGet("{id}")]
        public IActionResult Generate(Guid id)
        {
            return File(_captchaService.Generate(id), "image/png");
        }
    }
}
