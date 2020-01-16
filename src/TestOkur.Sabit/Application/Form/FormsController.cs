namespace TestOkur.Sabit.Application.Form
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Sabit.Extensions;

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
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            await file.SaveAsync(Path.Combine(_hostingEnvironment.WebRootPath, "forms"));

            return Accepted();
        }
    }
}
