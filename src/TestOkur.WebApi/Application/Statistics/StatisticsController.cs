namespace TestOkur.WebApi.Application.Statistics
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Darker;
	using TestOkur.Common;

	[Route("api/v1/statistics")]
	[Authorize(AuthorizationPolicies.Public)]
	public class StatisticsController : ControllerBase
    {
        private readonly IQueryProcessor _queryProcessor;

        public StatisticsController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(DailyReportStatisticsReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new DailyReportStatisticsQuery()));
        }
    }
}
