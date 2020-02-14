namespace TestOkur.WebApi.Application.Sms.Commands
{
    using MassTransit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Contracts.Sms;
    using TestOkur.Data;
    using TestOkur.Domain.Model.SmsModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class SendSmsCommandHandler : RequestHandlerAsync<SendSmsCommand>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ISmsCreditCalculator _smsCreditCalculator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendSmsCommandHandler(
            ISmsCreditCalculator smsCreditCalculator,
            IPublishEndpoint publishEndpoint,
            IHttpContextAccessor httpContextAccessor,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory dbContextFactory)
        {
            _smsCreditCalculator = smsCreditCalculator ??
                throw new ArgumentNullException(nameof(smsCreditCalculator));
            _publishEndpoint = publishEndpoint ??
                throw new ArgumentNullException(nameof(publishEndpoint));
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
            _queryProcessor = queryProcessor ??
                throw new ArgumentNullException(nameof(queryProcessor));
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<SendSmsCommand> HandleAsync(
            SendSmsCommand command,
            CancellationToken cancellationToken = default)
        {
            var messages = command.Messages
                .Select(m => new SmsMessage(m, _smsCreditCalculator.Calculate(m.Body)))
                .ToList();

            await DeductSmsCreditsAsync(messages, command.UserId, cancellationToken);
            await PublishEventAsync(command.UserId, messages, cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(int userId, IEnumerable<ISmsMessage> messages, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(userId, cancellationToken);
            var @event = new SendSmsRequestReceived(
                userId,
                _httpContextAccessor.GetUserId(),
                messages,
                user.Email);

            await _publishEndpoint.Publish<ISendSmsRequestReceived>(
                @event,
                cancellationToken);
        }

        private async Task<UserReadModel> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            var users = await _queryProcessor.ExecuteAsync(GetAllUsersQuery.Default, cancellationToken);
            var user = users.First(u => u.Id == userId);
            return user;
        }

        private async Task DeductSmsCreditsAsync(
            IEnumerable<SmsMessage> messages,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var totalCredit = messages.Sum(m => m.Credit);
            await using var dbContext = _dbContextFactory.Create(default);
            var user = await dbContext.Users.FirstAsync(l => l.Id == userId, cancellationToken);

            if (totalCredit > user.SmsBalance || user.SmsBalance <= 0)
            {
                throw new ValidationException(ErrorCodes.InsufficientFunds);
            }

            user.DeductSmsBalance(totalCredit);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
