namespace TestOkur.WebApi.Application.Cache
{
    using System.ComponentModel.DataAnnotations;
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.WebApi.Configuration;

    [Route("api/cache")]
    [AllowAnonymous]
    public class CacheController : ControllerBase
    {
        private readonly ICacheManager<object> _cacheManager;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public CacheController(ICacheManager<object> cacheManager, ApplicationConfiguration applicationConfiguration)
        {
            _cacheManager = cacheManager;
            _applicationConfiguration = applicationConfiguration;
        }

        [HttpDelete]
        public IActionResult Clear([FromQuery, Required] string key)
        {
            if (_applicationConfiguration.Key != key)
            {
                return Unauthorized();
            }

            _cacheManager.Clear();
            return NoContent();
        }
    }
}
