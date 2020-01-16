namespace TestOkur.Sabit.Application.Error
{
    using MassTransit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Sabit.Extensions;

    [Route("api/v1/error")]
    [Authorize(AuthorizationPolicies.Public)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPublishEndpoint _publishEndpoint;

        public ErrorController(IPublishEndpoint publishEndpoint, IWebHostEnvironment hostingEnvironment)
        {
            _publishEndpoint = publishEndpoint;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostAsync(ErrorModel model)
        {
            await _publishEndpoint.Publish(model);
            return Accepted();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            await file.SaveAsync(Path.Combine(_hostingEnvironment.WebRootPath, "uploads"));
            return Ok($@"\uploads\{file.FileName}");
        }
    }
}
