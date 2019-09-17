namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using IdentityModel;
    using MassTransit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Contracts.Sms;
    using TestOkur.Data;
    using TestOkur.Domain.Model.SmsModel;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class SendSmsCommandHandler : RequestHandlerAsync<SendSmsCommand>
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly ISmsCreditCalculator _smsCreditCalculator;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SendSmsCommandHandler(
			ISmsCreditCalculator smsCreditCalculator,
			IPublishEndpoint publishEndpoint,
			ApplicationDbContext applicationDbContext,
			IHttpContextAccessor httpContextAccessor,
			IQueryProcessor queryProcessor)
		{
			_smsCreditCalculator = smsCreditCalculator ??
				throw new ArgumentNullException(nameof(smsCreditCalculator));
			_publishEndpoint = publishEndpoint ??
				throw new ArgumentNullException(nameof(publishEndpoint));
			_applicationDbContext = applicationDbContext ??
				throw new ArgumentNullException(nameof(applicationDbContext));
			_httpContextAccessor = httpContextAccessor ??
				throw new ArgumentNullException(nameof(httpContextAccessor));
			_queryProcessor = queryProcessor ??
				throw new ArgumentNullException(nameof(queryProcessor));
		}

		[Idempotent(1)]
		public override async Task<SendSmsCommand> HandleAsync(
			SendSmsCommand command,
			CancellationToken cancellationToken = default)
		{
			var messages = command.Messages
				.Select(m => new SmsMessage(m, _smsCreditCalculator.Calculate(m.Body)));

			await EnsureBalanceIsSufficient(messages, command.UserId);
			await PublishEventAsync(command.UserId, messages, cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(int userId, IEnumerable<SmsMessage> messages, CancellationToken cancellationToken)
		{
			var user = await GetUserAsync(userId, cancellationToken);
			var @event = new SendSmsRequestReceived(
				userId,
				GetUserSubjectId(),
				messages,
				user.Email);

			await _publishEndpoint.Publish<ISendSmsRequestReceived>(
				@event,
				cancellationToken);
		}

		private async Task<UserReadModel> GetUserAsync(int userId, CancellationToken cancellationToken)
		{
			var users = await _queryProcessor.ExecuteAsync(new GetAllUsersQuery(), cancellationToken);
			var user = users.First(u => u.Id == userId);
			return user;
		}

		private async Task EnsureBalanceIsSufficient(IEnumerable<SmsMessage> messages, int userId)
		{
			var totalCredit = messages.Sum(m => m.Credit);
			if (totalCredit > await GetRemainingCredits(userId))
			{
				throw new ValidationException(ErrorCodes.InsufficientFunds);
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
