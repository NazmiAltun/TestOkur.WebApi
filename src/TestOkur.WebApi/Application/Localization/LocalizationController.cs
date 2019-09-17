namespace TestOkur.WebApi.Application.Localization
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using TestOkur.Common;

    [Route("api/v1/localization")]
    [Authorize(AuthorizationPolicies.Public)]
    public class LocalizationController : ControllerBase
	{
		private readonly IQueryProcessor _queryProcessor;

		public LocalizationController(IQueryProcessor queryProcessor)
		{
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpGet("{cultureCode}")]
		[ProducesResponseType(typeof(IReadOnlyCollection<LocalString>), StatusCodes.Status200OK)]
		public IActionResult Get(string cultureCode)
		{
			return Ok(_queryProcessor.Execute(new GetLocalStringsQuery(cultureCode)));
		}
	}
}
