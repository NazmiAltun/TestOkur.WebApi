namespace TestOkur.WebApi.Application.Exam
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.Common;
	using TestOkur.WebApi.Application.Exam.Commands;
	using TestOkur.WebApi.Application.Exam.Queries;

	[Route("api/v1/exams")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class ExamController : Controller
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly IAmACommandProcessor _commandProcessor;

		public ExamController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody, Required]CreateExamCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<ExamReadModel>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAsync()
		{
			return Ok(await _queryProcessor.ExecuteAsync(new GetUserExamsQuery()));
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			await _commandProcessor.SendAsync(new DeleteExamCommand(id));
			return Ok();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> EditAsync([FromBody, Required]EditExamCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}
	}
}
