namespace TestOkur.Report.Controllers
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading.Tasks;
	using MassTransit;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using TestOkur.Common;
	using TestOkur.Optic.Form;
	using TestOkur.Report.Events;
	using TestOkur.Report.Extensions;
	using TestOkur.Report.Repositories;

	[Route("api/v1/forms")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class OpticalFormController : Controller
	{
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IOpticalFormRepository _opticalFormRepository;

		public OpticalFormController(
			IOpticalFormRepository opticalFormRepository,
			IHttpContextAccessor httpContextAccessor,
			IPublishEndpoint publishEndpoint)
		{
			_opticalFormRepository = opticalFormRepository;
			_httpContextAccessor = httpContextAccessor;
			_publishEndpoint = publishEndpoint;
		}

		[HttpGet("exam/answer/{examId}")]
		[ProducesResponseType(typeof(IReadOnlyCollection<AnswerKeyOpticalForm>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAnswerKeyFormsAsync(int examId)
		{
			return Ok(await _opticalFormRepository.GetAnswerKeyOpticalForms(examId));
		}

		[HttpGet("exam/student/{examId}")]
		[ProducesResponseType(typeof(IReadOnlyCollection<StudentOpticalForm>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetStudentFormsAsync(int examId)
		{
			var forms = await _opticalFormRepository.GetStudentOpticalFormAsync(examId);

			return Ok(forms.Where(f => f.UserId == _httpContextAccessor.GetUserId()));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> AddManyAsync([FromBody, Required]IEnumerable<StudentOpticalForm> forms)
		{
			await _opticalFormRepository.DeleteManyAsync(forms);
			await _opticalFormRepository.AddManyAsync(forms);
			await _publishEndpoint.Publish(new EvaluateExam(forms.First().ExamId));
			return Ok();
		}
	}
}
