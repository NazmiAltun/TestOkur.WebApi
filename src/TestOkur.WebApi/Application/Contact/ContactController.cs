namespace TestOkur.WebApi.Application.Contact
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Common;

    [Route("api/v1/contacts")]
    [ApiController]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ContactController : ControllerBase
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public ContactController(IAmACommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _queryProcessor = queryProcessor;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditAsync(BulkEditContactsCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateContactCommand command)
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
