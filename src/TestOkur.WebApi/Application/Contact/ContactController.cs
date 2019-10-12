namespace TestOkur.WebApi.Application.Contact
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Infrastructure.CommandsQueries;

    [Route("api/v1/contacts")]
    [ApiController]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ContactController : ControllerBase
    {
        private readonly IProcessor _processor;

        public ContactController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditAsync(BulkEditContactsCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateContactCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ContactReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetUserContactsQuery, IReadOnlyCollection<ContactReadModel>>(new GetUserContactsQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteContactCommand(id));
            return Ok();
        }
    }
}
