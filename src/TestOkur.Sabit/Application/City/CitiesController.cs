namespace TestOkur.Sabit.Application.City
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Common;

    [Route("api/v1/cities")]
    [Authorize(AuthorizationPolicies.Public)]
    public class CitiesController : ControllerBase
    {
        public CitiesController()
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<Sabit.Application.City.City>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok();
        }
    }
}
