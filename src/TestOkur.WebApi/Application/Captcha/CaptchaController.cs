namespace TestOkur.WebApi.Application.Captcha
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
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
        public async Task<IActionResult> GenerateAsync(Guid id)
        {
            return File(
                await _captchaService.GenerateAsync(id),
                "image/png");
        }
    }
}
