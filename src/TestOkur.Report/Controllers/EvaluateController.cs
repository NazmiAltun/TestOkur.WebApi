namespace TestOkur.Report.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Infrastructure.Repositories;

    [Route("api/v1/evaluate")]
    [Authorize(AuthorizationPolicies.Customer)]
    [ApiController]
    public class EvaluateController : ControllerBase
    {
        private readonly IEvaluator _evaluator;
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public EvaluateController(
            IEvaluator evaluator,
            IStudentOpticalFormRepository studentOpticalFormRepository,
            IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _evaluator = evaluator;
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<StudentOpticalForm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinAndEvaluateAsync(
            [FromQuery] int primaryExamId,
            [FromQuery] int secondaryExamId)
        {
            var primaryExamForms = await _studentOpticalFormRepository.GetStudentOpticalFormsByExamIdAsync(primaryExamId);
            var secondaryExamForms = await _studentOpticalFormRepository.GetStudentOpticalFormsByExamIdAsync(secondaryExamId);
            var combinedExamForms = _evaluator.JoinSets(primaryExamForms, secondaryExamForms);
            var answerKeyOpticalForms = await _answerKeyOpticalFormRepository.GetByExamIdAsync(primaryExamId);

            return Ok(_evaluator.Evaluate(answerKeyOpticalForms.ToList(), combinedExamForms.ToList()));
        }
    }
}
