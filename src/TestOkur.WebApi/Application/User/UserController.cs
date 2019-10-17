namespace TestOkur.WebApi.Application.User
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Domain;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Commands;
    using TestOkur.WebApi.Application.User.Queries;

    [Route("api/v1/users")]
    [ApiController]
    public sealed class UserController : ControllerBase
    {
        private readonly IProcessor _processor;

        public UserController(IProcessor processor)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        [HttpPost("send-reset-password-link")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Public)]
        public async Task<IActionResult> SendResetPasswordLinkAsync(SendResetPasswordLinkCommand command)
        {
            await _processor.SendAsync(command);

            return Ok(SuccessCodes.PasswordResetLinkSent);
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Customer)]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserCommand command)
        {
            await _processor.SendAsync(command);

            return Ok(SuccessCodes.UserUpdated);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Public)]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
        {
            await _processor.SendAsync(command);

            return Ok(SuccessCodes.UserCreated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthorizationPolicies.Admin)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _processor.SendAsync(new DeleteUserCommand(id));
            return Ok();
        }

        [HttpPost("activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Admin)]
        public async Task<IActionResult> ActivateAsync([FromQuery, Required]string email)
        {
            var command = new ActivateUserCommand(email);
            await _processor.SendAsync(command);
            return Ok(SuccessCodes.UserActivated);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserReadModel>), StatusCodes.Status200OK)]
        [Authorize(AuthorizationPolicies.Private)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _processor
                .ExecuteAsync<GetAllUsersQuery, IReadOnlyCollection<UserReadModel>>(
                    new GetAllUsersQuery());

            return Ok(users);
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(UserReadModel), StatusCodes.Status200OK)]
        [Authorize(AuthorizationPolicies.Admin)]
        public async Task<IActionResult> GetUserAsync(string email)
        {
            var user = await _processor
                .ExecuteAsync<GetUserByEmailQuery, UserReadModel>(
                    new GetUserByEmailQuery(email));

            return Ok(user);
        }

        [HttpGet("me")]
        [Authorize(AuthorizationPolicies.Customer)]
        [ProducesResponseType(typeof(UserReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestedUserInfoAsync([FromQuery]string version)
        {
            var user = await _processor
                .ExecuteAsync<GetUserByEmailQuery, UserReadModel>(
                    new GetUserByEmailQuery());

            await _processor.SendAsync(new UpdateUserOnlineStatusCommand(user.Email));
            return Ok(user);
        }

        [HttpGet("record-counts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthorizationPolicies.Customer)]
        public async Task<IActionResult> GetUserRecordCountsAsync()
        {
            var records = await _processor.ExecuteAsync<GetUserRecordCountsQuery, UserRecords>(new GetUserRecordCountsQuery());

            return Ok(records);
        }

        [HttpPost("update-by-admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Admin)]
        public async Task<IActionResult> UpdateUserByAdminAsync(UpdateUserByAdminCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpPost("extend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(AuthorizationPolicies.Admin)]
        public async Task<IActionResult> ExtendSubscriptionAsync(ExtendUserSubscriptionCommand command)
        {
            await _processor.SendAsync(command);
            return Ok();
        }

        [HttpGet("online")]
        [Authorize(AuthorizationPolicies.Admin)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOnlineUsersAsync()
        {
            return Ok(await _processor.ExecuteAsync<GetOnlineUsersQuery, IReadOnlyCollection<string>>(
                new GetOnlineUsersQuery()));
        }
    }
}
