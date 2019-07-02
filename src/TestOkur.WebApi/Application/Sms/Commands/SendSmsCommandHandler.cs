namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using IdentityModel;
	using MassTransit;
	using Microsoft.AspNetCore.Http;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Common;
	using TestOkur.Contracts.Sms;
	using TestOkur.Data;
	using TestOkur.Domain.Model.SmsModel;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class SendSmsCommandHandler : RequestHandlerAsync<SendSmsCommand>
	{
		private readonly ISmsCreditCalculator _smsCreditCalculator;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SendSmsCommandHandler(
			ISmsCreditCalculator smsCreditCalculator,
			IPublishEndpoint publishEndpoint,
			ApplicationDbContext applicationDbContext,
			IHttpContextAccessor httpContextAccessor)
		{
			_smsCreditCalculator = smsCreditCalculator ??
				throw new ArgumentNullException(nameof(smsCreditCalculator));
			_publishEndpoint = publishEndpoint ??
				throw new ArgumentNullException(nameof(publishEndpoint));
			_applicationDbContext = applicationDbContext ??
				throw new ArgumentNullException(nameof(applicationDbContext));
			_httpContextAccessor = httpContextAccessor ??
				throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		[Idempotent(1)]
		[Populate(2)]
		public override async Task<SendSmsCommand> HandleAsync(
			SendSmsCommand command,
			CancellationToken cancellationToken = default)
		{
			SetSmsCredits(command);
			await EnsureBalanceIsSufficient(command);
			await PublishEventAsync(command, cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(SendSmsCommand command, CancellationToken cancellationToken)
		{
			var @event = new SendSmsRequestReceived(
				command.UserId,
				GetUserSubjectId(),
				command.Messages);

			await _publishEndpoint.Publish<ISendSmsRequestReceived>(
				@event,
				cancellationToken);
		}

		private async Task EnsureBalanceIsSufficient(SendSmsCommand command)
		{
			var totalCredit = command.Messages.Select(m => m.Credit)
				.Sum();
			if (totalCredit > await GetRemainingCredits(command.UserId))
			{
				throw new ValidationException(ErrorCodes.InsufficientFunds);
			}
		}

		private void SetSmsCredits(SendSmsCommand command)
		{
			foreach (var message in command.Messages)
			{
				message.Credit = _smsCreditCalculator.Calculate(message.Body);
				message.Id = Guid.NewGuid();
			}
		}

		private string GetUserSubjectId()
		{
			return _httpContextAccessor.HttpContext?.User?
				.FindFirst(JwtClaimTypes.Subject)?.Value;
		}

		private async Task<int> GetRemainingCredits(int userId)
		{
			return (await _applicationDbContext.Users
				.FirstAsync(l => l.Id == userId)).SmsBalance;
		}
	}
}
