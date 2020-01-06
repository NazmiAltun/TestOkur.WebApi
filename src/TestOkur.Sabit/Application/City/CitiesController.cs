namespace TestOkur.Sabit.Application.City
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Common;

    [Route("api/v1/cities")]
    [Authorize(AuthorizationPolicies.Public)]
    public class CitiesController : ControllerBase
    {
        private readonly IQueryProcessor _queryProcessor;

        public CitiesController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<City>), StatusCodes.Status200OK)]
        [ResponseCache(Duration = 100000)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(CityQuery.Default));
        }
    }
}
