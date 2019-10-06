namespace TestOkur.WebApi.Application.Cache
{
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/cache")]
    [AllowAnonymous]
    public class CacheController : ControllerBase
    {
        private readonly ICacheManager<object> _cacheManager;

        public CacheController(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        [HttpDelete]
        public IActionResult Clear()
        {
            _cacheManager.Clear();
            return NoContent();
        }
    }
}
