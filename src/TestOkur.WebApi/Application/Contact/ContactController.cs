namespace TestOkur.WebApi.Application.Contact
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Infrastructure.Cqrs;

    [Route("api/v1/contacts")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ContactController : ControllerBase
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly IContextCommandProcessor _commandProcessor;

		public ContactController(IQueryProcessor queryProcessor, IContextCommandProcessor commandProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> EditAsync([FromBody, Required]BulkEditContactsCommand command)
		{
			await _commandProcessor.ExecuteAsync(command);
			return Ok();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody, Required]CreateContactCommand command)
		{
			await _commandProcessor.ExecuteAsync(command);
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
			await _commandProcessor.ExecuteAsync(new DeleteContactCommand(id));
			return Ok();
		}
	}
}
