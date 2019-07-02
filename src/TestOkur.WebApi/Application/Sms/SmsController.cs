namespace TestOkur.WebApi.Application.Sms
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.Common;
	using TestOkur.WebApi.Application.Sms.Commands;

	[Route("api/v1/sms")]
	public class SmsController : Controller
    {
        private readonly IAmACommandProcessor _commandProcessor;

        public SmsController(IAmACommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        [HttpPost]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendSmsAsync([FromBody, Required]SendSmsCommand command)
        {
            await _commandProcessor.SendAsync(command);

            return Ok();
        }

        [HttpPost("deduct-credits")]
        [Authorize(AuthorizationPolicies.Private)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeductSmsCreditsAsync([FromBody, Required]DeductSmsCreditsCommand command)
        {
            await _commandProcessor.SendAsync(command);

            return Ok();
        }

        [HttpPost("add-credits")]
        [Authorize(AuthorizationPolicies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSmsCreditsAsync([FromBody, Required]AddSmsCreditsCommand command)
        {
	        await _commandProcessor.SendAsync(command);

	        return Ok();
        }
	}
}
