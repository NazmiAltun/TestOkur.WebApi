namespace TestOkur.WebApi.Application.Student
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

	[Route("api/v1/students")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class StudentController : Controller
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IAmACommandProcessor _commandProcessor;

        public StudentController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody, Required]CreateStudentCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
	    {
		    await _commandProcessor.SendAsync(new DeleteStudentCommand(id));
		    return Ok();
	    }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsync([FromBody, Required]EditStudentCommand command)
	    {
		    await _commandProcessor.SendAsync(command);
		    return Ok();
	    }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<StudentReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
	    {
		    return Ok(await _queryProcessor.ExecuteAsync(new GetUserStudentsQuery()));
	    }

        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBulkAsync([FromBody, Required]BulkCreateStudentCommand command)
	    {
		    await _commandProcessor.SendAsync(command);

		    return Ok();
	    }
	}
}
