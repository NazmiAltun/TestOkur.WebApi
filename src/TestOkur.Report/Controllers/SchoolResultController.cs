namespace TestOkur.Report.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Infrastructure.Repositories;

    [Route("api/v1/school-results")]
    [Authorize(AuthorizationPolicies.Admin)]
    public class SchoolResultController : ControllerBase
    {
        private readonly ISchoolResultRepository _schoolResultRepository;

        public SchoolResultController(ISchoolResultRepository schoolResultRepository)
        {
            _schoolResultRepository = schoolResultRepository;
        }

        [HttpGet("{examId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<SchoolResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnswerKeyFormsAsync(int examId)
        {
            return Ok(await _schoolResultRepository.GetByExamId(examId));
        }
    }
}
