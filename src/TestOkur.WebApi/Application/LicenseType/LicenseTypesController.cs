namespace TestOkur.WebApi.Application.LicenseType
{
	using System;
	using System.Collections.Generic;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Darker;
	using TestOkur.Common;

	[Route("api/v1/license-types")]
	[Authorize(AuthorizationPolicies.Public)]
	public class LicenseTypesController : ControllerBase
	{
		private readonly IQueryProcessor _queryProcessor;

		public LicenseTypesController(IQueryProcessor queryProcessor)
		{
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<LicenseTypeReadModel>), StatusCodes.Status200OK)]
		public IActionResult Get()
		{
			return Ok(_queryProcessor.Execute(new GetAllLicenseTypesQuery()));
		}
	}
}
