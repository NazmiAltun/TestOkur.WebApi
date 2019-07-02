namespace TestOkur.WebApi.Application.Settings
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Darker;
	using TestOkur.Common;

	[Route("api/v1/settings")]
	[Authorize(AuthorizationPolicies.Private)]
	public class SettingsController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;

        public SettingsController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet("appsettings")]
        [ProducesResponseType(typeof(IReadOnlyCollection<AppSettingReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAppSettingsAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new GetAllAppSettingsQuery()));
        }
    }
}
