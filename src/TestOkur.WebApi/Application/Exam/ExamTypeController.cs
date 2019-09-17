namespace TestOkur.WebApi.Application.Exam
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.WebApi.Application.Exam.Queries;

    [Route("api/v1/exam-types")]
    [Authorize(AuthorizationPolicies.Public)]
    public class ExamTypeController : ControllerBase
    {
        private readonly IQueryProcessor _queryProcessor;

        public ExamTypeController(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ExamTypeReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _queryProcessor.ExecuteAsync(new ExamTypeQuery()));
        }
    }
}
