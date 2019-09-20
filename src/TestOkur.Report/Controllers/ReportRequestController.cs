namespace TestOkur.Report.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Report.Models;
    using TestOkur.Report.Repositories;

    [Route("api/v1/report-requests")]
    public class ReportRequestController : ControllerBase
    {
        private readonly IReportRequestRepository _reportRequestRepository;

        public ReportRequestController(IReportRequestRepository reportRequestRepository)
        {
            _reportRequestRepository = reportRequestRepository;
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAsync([FromBody, Required]ReportRequest reportRequest)
        {
            await _reportRequestRepository.AddAsync(reportRequest);
            return Ok();
        }

        [HttpGet]
        [Authorize(AuthorizationPolicies.Private)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            return Ok(await _reportRequestRepository.GetStatisticsAsync());
        }
    }
}
