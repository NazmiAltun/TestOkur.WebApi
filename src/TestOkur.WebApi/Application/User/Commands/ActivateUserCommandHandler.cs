namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Application.User.Services;

    public sealed class ActivateUserCommandHandler : RequestHandlerAsync<ActivateUserCommand>
    {
        private const int ReferrerGainedSmsCredits = 1000;
        private const int RefereeGainedSmsCredits = 500;

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _identityService;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IApplicationDbContextFactory _applicationDbContextFactory;

        public ActivateUserCommandHandler(
            IPublishEndpoint publishEndpoint,
            IIdentityService identityService,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory applicationDbContextFactory)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _applicationDbContextFactory = applicationDbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<ActivateUserCommand> HandleAsync(
            ActivateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _queryProcessor.ExecuteAsync(
                new GetUserQuery(command.Email), cancellationToken);
            await _identityService.ActivateUserAsync(command.Email, cancellationToken);
            await PublishUserActivatedEventAsync(user, cancellationToken);
            await ApplyPromotionAsync(user, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task ApplyPromotionAsync(
            UserReadModel user,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Referrer))
            {
                return;
            }

            await using var dbContext = _applicationDbContextFactory.Create(user.Id);
            var referee = await dbContext.Users.FirstAsync(u => u.Id == user.Id, cancellationToken);
            var referrer = await dbContext.Users.FirstAsync(u => u.Email.Value == user.Referrer, cancellationToken);
            referrer.AddSmsBalance(ReferrerGainedSmsCredits);
            referee.AddSmsBalance(RefereeGainedSmsCredits);
            await dbContext.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(
                new ReferredUserActivated(
                    referee,
                    referrer,
                    RefereeGainedSmsCredits,
                    ReferrerGainedSmsCredits), cancellationToken);
        }

        private async Task PublishUserActivatedEventAsync(
            UserReadModel user,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new UserActivated(
                    user.Phone,
                    user.Email,
                    user.LastName,
                    user.FirstName), cancellationToken);
        }
    }
}
