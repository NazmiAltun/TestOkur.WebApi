namespace TestOkur.WebApi.Application.LicenseType
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Darker;
	using TestOkur.Common;

	[Route("api/v1/license-types")]
	[Authorize(AuthorizationPolicies.Public)]
	public class LicenseTypesController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public LicenseTypesController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<LicenseTypeReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetAllLicenseTypesQuery()));
        }
    }
}
