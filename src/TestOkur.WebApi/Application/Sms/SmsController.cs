namespace TestOkur.WebApi.Application.Sms
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using TestOkur.Common;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Sms.Commands;

    [Route("api/v1/sms")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IProcessor _contextCommandProcessor;

        public SmsController(IAmACommandProcessor commandProcessor, IProcessor contextCommandProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _contextCommandProcessor = contextCommandProcessor;
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendSmsAsync(SendSmsCommand command)
        {
            await _contextCommandProcessor.SendAsync(command);

            return Accepted();
        }

        [HttpPost("send-admin")]
        [Authorize(AuthorizationPolicies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendSmsAsync(SendSmsAdminCommand command)
        {
            await _contextCommandProcessor.SendAsync(command);
            return Accepted();
        }

        [HttpPost("deduct-credits")]
        [Authorize(AuthorizationPolicies.Private)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeductSmsCreditsAsync(DeductSmsCreditsCommand command)
        {
            await _commandProcessor.SendAsync(command);

            return Ok();
        }

        [HttpPost("add-credits")]
        [Authorize(AuthorizationPolicies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSmsCreditsAsync(AddSmsCreditsCommand command)
        {
            await _commandProcessor.SendAsync(command);

            return Ok();
        }
    }
}
