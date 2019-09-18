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
    [Authorize(AuthorizationPolicies.Customer)]
    public class ReportRequestController : ControllerBase
    {
        private readonly IReportRequestRepository _reportRequestRepository;

        public ReportRequestController(IReportRequestRepository reportRequestRepository)
        {
            _reportRequestRepository = reportRequestRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAsync([FromBody, Required]ReportRequest reportRequest)
        {
            await _reportRequestRepository.AddAsync(reportRequest);
            return Ok();
        }
    }
}
