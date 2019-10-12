namespace TestOkur.Notification.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Common;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    [Route("api/v1/emails")]
    [Authorize(AuthorizationPolicies.Admin)]
    public class EmailController : ControllerBase
    {
        private readonly IEMailRepository _eMailRepository;

        public EmailController(IEMailRepository eMailRepository)
        {
            _eMailRepository = eMailRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<EMail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(
            [FromQuery, Required] DateTime from,
            [FromQuery, Required] DateTime to)
        {
            return Ok(await _eMailRepository.GetEmailsAsync(from, to));
        }
    }
}
