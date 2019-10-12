namespace TestOkur.WebApi.Application.Cache
{
    using System.ComponentModel.DataAnnotations;
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using TestOkur.WebApi.Configuration;

    [Route("api/cache")]
    [AllowAnonymous]
    public class CacheController : ControllerBase
    {
        private readonly ICacheManager<object> _cacheManager;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CacheController(ICacheManager<object> cacheManager, ApplicationConfiguration applicationConfiguration, IWebHostEnvironment webHostEnvironment)
        {
            _cacheManager = cacheManager;
            _applicationConfiguration = applicationConfiguration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpDelete]
        public IActionResult Clear([FromQuery, Required] string key)
        {
            if (_applicationConfiguration.Key != key)
            {
                return Unauthorized();
            }

            if (!_webHostEnvironment.IsDevelopment())
            {
                _cacheManager.Clear();
            }

            return NoContent();
        }
    }
}
