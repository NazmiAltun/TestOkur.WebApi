namespace TestOkur.Sabit.Application.Form
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;

    [Route("api/v1/forms")]
    [Authorize(AuthorizationPolicies.Public)]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FormsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            await SaveFileAsync(file);
            return Accepted();
        }

        private async Task SaveFileAsync(IFormFile file)
        {
            var path = Path.Combine(
                _hostingEnvironment.WebRootPath,
                "forms",
                file.FileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}
