namespace TestOkur.WebApi.Application.Cache
{
    using CacheManager.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
        public async Task<IActionResult> ClearAsync()
        {
            _cacheManager.Clear();
            return NoContent();
        }
    }
}
