using System.Collections.Generic;

namespace TestOkur.WebApi.Application.User
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
	using TestOkur.Domain;
	using TestOkur.WebApi.Application.User.Commands;
	using TestOkur.WebApi.Application.User.Queries;

	[Route("api/v1/users")]
	public sealed class UserController : Controller
	{
		private readonly IAmACommandProcessor _commandProcessor;
		private readonly IQueryProcessor _queryProcessor;

		public UserController(
			IAmACommandProcessor commandProcessor,
			IQueryProcessor queryProcessor)
		{
			_commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		[HttpPost("send-reset-password-link")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(AuthorizationPolicies.Public)]
		public async Task<IActionResult> SendResetPasswordLinkAsync([FromBody, Required]SendResetPasswordLinkCommand command)
		{
			await _commandProcessor.SendAsync(command);

			return Ok(SuccessCodes.PasswordResetLinkSent);
		}

		[HttpPut]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(AuthorizationPolicies.Customer)]
		public async Task<IActionResult> UpdateUserAsync([FromBody, Required]UpdateUserCommand command)
		{
			await _commandProcessor.SendAsync(command);

			return Ok(SuccessCodes.UserUpdated);
		}

		[HttpPost]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(AuthorizationPolicies.Public)]
		public async Task<IActionResult> CreateUserAsync([FromBody, Required]CreateUserCommand command)
		{
			await _commandProcessor.SendAsync(command);

			return Ok(SuccessCodes.UserCreated);
		}

		[HttpPost("activate")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(AuthorizationPolicies.Admin)]
		public async Task<IActionResult> ActivateAsync([FromQuery, Required]string email)
		{
			var command = new ActivateUserCommand(email);
			await _commandProcessor.SendAsync(command);
			return Ok(SuccessCodes.UserActivated);
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<UserReadModel>), StatusCodes.Status200OK)]
		[Authorize(AuthorizationPolicies.Admin)]
		public async Task<IActionResult> GetUsersAsync()
		{
			var users = await _queryProcessor.ExecuteAsync(new GetAllUsersQuery());

			return Ok(users);
		}

		[HttpGet("{email}")]
		[ProducesResponseType(typeof(UserReadModel), StatusCodes.Status200OK)]
		[Authorize(AuthorizationPolicies.Admin)]
		public async Task<IActionResult> GetUserAsync(string email)
		{
			var user = await _queryProcessor.ExecuteAsync(new GetUserByEmailQuery(email));

			return Ok(user);
		}

		[HttpGet("me")]
		[ProducesResponseType(typeof(UserReadModel), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetUserAsync()
		{
			var user = await _queryProcessor.ExecuteAsync(new GetUserByEmailQuery());

			return Ok(user);
		}

		[HttpGet("record-counts")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[Authorize(AuthorizationPolicies.Customer)]
		public async Task<IActionResult> GetUserRecordCountsAsync()
		{
			var records = await _queryProcessor.ExecuteAsync(new GetUserRecordCountsQuery());

			return Ok(records);
		}

		[HttpPost("update-by-admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(AuthorizationPolicies.Admin)]
		public async Task<IActionResult> UpdateUserByAdminAsync([FromBody, Required]UpdateUserByAdminCommand command)
		{
			await _commandProcessor.SendAsync(command);
			return Ok();
		}
	}
}
