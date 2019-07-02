namespace TestOkur.WebApi.Application.Contact
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

	[Route("api/v1/contacts")]
	[Authorize(AuthorizationPolicies.Customer)]
	public class ContactController : Controller
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly IAmACommandProcessor _commandProcessor;

		public ContactController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> EditAsync([FromBody, Required]BulkEditContactsCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody, Required]CreateContactCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<ContactReadModel>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAsync()
		{
			return Ok(await _queryProcessor.ExecuteAsync(new GetUserContactsQuery()));
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			await _commandProcessor.SendAsync(new DeleteContactCommand(id));
			return Ok();
		}
	}
}
