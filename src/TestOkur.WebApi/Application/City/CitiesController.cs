namespace TestOkur.WebApi.Application.City
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Darker;
	using TestOkur.Common;

	[Route("api/v1/cities")]
	[Authorize(AuthorizationPolicies.Public)]
	public class CitiesController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public CitiesController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<CityReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetAllCitiesQuery()));
        }
    }
}
