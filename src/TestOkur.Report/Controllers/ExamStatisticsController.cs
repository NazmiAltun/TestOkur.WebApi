namespace TestOkur.Report.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Report.Domain.Statistics;
    using TestOkur.Report.Infrastructure.Repositories;

    [Route("api/v1/exam-statistics")]
    [ApiController]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ExamStatisticsController : ControllerBase
    {
        private readonly IExamStatisticsRepository _examStatisticsRepository;

        public ExamStatisticsController(IExamStatisticsRepository examStatisticsRepository)
        {
            _examStatisticsRepository = examStatisticsRepository;
        }

        [HttpGet("{examId}")]
        [ProducesResponseType(typeof(ExamStatistics), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(int examId)
        {
            var stats = await _examStatisticsRepository.GetAsync(examId);
            return Ok(stats ?? ExamStatistics.Empty);
        }
    }
}
