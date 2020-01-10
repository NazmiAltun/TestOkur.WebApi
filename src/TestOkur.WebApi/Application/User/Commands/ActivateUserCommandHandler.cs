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
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class ActivateUserCommandHandler : RequestHandlerAsync<ActivateUserCommand>
    {
        private const int ReferrerGainedSmsCredits = 1000;
        private const int RefereeGainedSmsCredits = 500;

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityClient _identityClient;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IApplicationDbContextFactory _applicationDbContextFactory;

        public ActivateUserCommandHandler(
            IPublishEndpoint publishEndpoint,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory applicationDbContextFactory)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
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
            await _identityClient.ActivateUserAsync(command.Email, cancellationToken);
            await PublishUserActivatedEventAsync(user, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
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
                    user.FirstName,
                    user.Id,
                    user.SubjectId), cancellationToken);
        }
    }
}
