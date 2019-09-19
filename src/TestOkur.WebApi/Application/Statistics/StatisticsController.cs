namespace TestOkur.WebApi.Application.Statistics
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using System;
    using System.Threading.Tasks;
    using TestOkur.Common;

    [Route("api/v1/statistics")]
    [Authorize(AuthorizationPolicies.Private)]
    public class StatisticsController : ControllerBase
    {
        private readonly IQueryProcessor _queryProcessor;

        public StatisticsController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(StatisticsReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new StatisticsQuery()));
        }
    }
}
