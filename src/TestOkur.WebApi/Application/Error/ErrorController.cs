namespace TestOkur.WebApi.Application.Error
{
    using System.IO;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;

    [Route("api/v1/error")]
    [Authorize(AuthorizationPolicies.Customer)]
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
            await SaveImageAsync(file);
            return Ok($@"\uploads\{file.FileName}");
        }

        private async Task SaveImageAsync(IFormFile file)
        {
            var path = Path.Combine(
                _hostingEnvironment.WebRootPath,
                "uploads",
                file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
