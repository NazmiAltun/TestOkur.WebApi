namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Infrastructure.CommandsQueries;

    [Route("api/v1/scan-sessions")]
    [ApiController]
    [Authorize(AuthorizationPolicies.Customer)]
    public class ScanController : ControllerBase
    {
        private readonly IProcessor _commandProcessor;

        public ScanController(IProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> StartAsync(StartScanSessionCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EndAsync(EndScanSessionCommand command)
        {
            await _commandProcessor.SendAsync(command);
            return Ok();
        }
    }
}
