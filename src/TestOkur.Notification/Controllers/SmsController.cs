namespace TestOkur.Notification.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    [Route("api/v1/smses")]
    [Authorize(AuthorizationPolicies.Customer)]
    public class SmsController : ControllerBase
    {
        private readonly ISmsRepository _smsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmsController(ISmsRepository smsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _smsRepository = smsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserSmsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserSmses()
        {
            var smses = await _smsRepository.GetUserSmsesAsync(_httpContextAccessor.GetUserId());
            return Ok(smses.Select(s => new UserSmsModel(s)));
        }
    }
}
