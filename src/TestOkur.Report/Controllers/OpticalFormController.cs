namespace TestOkur.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Events;
    using TestOkur.Report.Extensions;
    using TestOkur.Report.Infrastructure.Repositories;

    [Route("api/v1/forms")]
    [ApiController]
    [Authorize(AuthorizationPolicies.Customer)]
    public class OpticalFormController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentOpticalFormRepository _studentOpticalFormRepository;
        private readonly IAnswerKeyOpticalFormRepository _answerKeyOpticalFormRepository;

        public OpticalFormController(
            IStudentOpticalFormRepository studentOpticalFormRepository,
            IHttpContextAccessor httpContextAccessor,
            IBus bus,
            IAnswerKeyOpticalFormRepository answerKeyOpticalFormRepository)
        {
            _studentOpticalFormRepository = studentOpticalFormRepository;
            _httpContextAccessor = httpContextAccessor;
            _bus = bus;
            _answerKeyOpticalFormRepository = answerKeyOpticalFormRepository;
        }

        [HttpGet("exam/answer/{examId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<AnswerKeyOpticalForm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAnswerKeyFormsAsync(int examId)
        {
            return Ok(await _answerKeyOpticalFormRepository.GetByExamIdAsync(examId));
        }

        [HttpGet("exam/student/{examId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<StudentOpticalForm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentFormsByExamIdAsync(int examId)
        {
            return _httpContextAccessor.CheckIfAdmin()
                ? Ok(await _studentOpticalFormRepository.GetStudentOpticalFormsByExamIdAsync(examId))
                : Ok(await _studentOpticalFormRepository.GetStudentOpticalFormsByExamIdAsync(
                examId,
                _httpContextAccessor.GetUserId()));
        }

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<StudentOpticalForm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentFormsAsync(int studentId)
        {
            var forms = await _studentOpticalFormRepository.GetStudentOpticalByStudentIdAsync(studentId);

            return Ok(forms.Where(f => f.UserId == _httpContextAccessor.GetUserId() ||
                                       _httpContextAccessor.CheckIfAdmin()));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddManyAsync(IEnumerable<StudentOpticalForm> forms)
        {
            await _studentOpticalFormRepository.DeleteManyAsync(forms);
            await _studentOpticalFormRepository.AddManyAsync(forms);
            await _bus.Publish(new EvaluateExam(forms.First().ExamId));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var deletedForm = await _studentOpticalFormRepository.DeleteOneAsync(id);

            if (deletedForm != null)
            {
                await _bus.Publish(new EvaluateExam(deletedForm.ExamId));
            }

            return Ok();
        }
    }
}
