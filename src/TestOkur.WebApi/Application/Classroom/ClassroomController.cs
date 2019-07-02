namespace TestOkur.WebApi.Application.Classroom
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
	using TestOkur.Domain;

	[Route("api/v1/classrooms")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class ClassroomController : Controller
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly IAmACommandProcessor _commandProcessor;

		public ClassroomController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody, Required]CreateClassroomCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok(SuccessCodes.ClassCreated);
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<ClassroomReadModel>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAsync()
		{
			return Ok(await _queryProcessor.ExecuteAsync(new GetUserClassroomsQuery()));
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			await _commandProcessor.SendAsync(new DeleteClassroomCommand(id));
			return Ok();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> EditAsync([FromBody, Required]EditClassroomCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}
	}
}
