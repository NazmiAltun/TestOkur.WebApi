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
    using TestOkur.Report.Repositories;

    [Route("api/v1/evaluate")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class EvaluateController : ControllerBase
    {
        private readonly IEvaluator _evaluator;
        private readonly IOpticalFormRepository _opticalFormRepository;

        public EvaluateController(IEvaluator evaluator, IOpticalFormRepository opticalFormRepository)
        {
            _evaluator = evaluator;
            _opticalFormRepository = opticalFormRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IReadOnlyCollection<StudentOpticalForm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinAndEvaluateAsync(int primaryExamId, int secondaryExamId)
        {
            var primaryExamForms = await _opticalFormRepository.GetStudentOpticalFormsByExamIdAsync(primaryExamId);
            var secondaryExamForms = await _opticalFormRepository.GetStudentOpticalFormsByExamIdAsync(secondaryExamId);
            var combinedExamForms = _evaluator.JoinSets(primaryExamForms, secondaryExamForms);
            var answerKeyOpticalForms = await _opticalFormRepository.GetAnswerKeyOpticalForms(primaryExamId);

            return Ok(_evaluator.Evaluate(answerKeyOpticalForms.ToList(), combinedExamForms.ToList()));
        }
    }
}
