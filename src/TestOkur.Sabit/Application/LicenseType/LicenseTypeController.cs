namespace TestOkur.Sabit.Application.LicenseType
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Common;

    [Route("api/v1/license-types")]
    [Authorize(AuthorizationPolicies.Public)]
    public class LicenseTypeController : ControllerBase
    {
        private readonly IQueryProcessor _queryProcessor;

        public LicenseTypeController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LicenseType>), StatusCodes.Status200OK)]
        [ResponseCache(Duration = 100000)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new LicenseTypeQuery()));
        }
    }
}
