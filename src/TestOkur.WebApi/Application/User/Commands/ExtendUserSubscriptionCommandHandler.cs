namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class ExtendUserSubscriptionCommandHandler : RequestHandlerAsync<ExtendUserSubscriptionCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityClient _identityClient;
        private readonly IQueryProcessor _queryProcessor;

        public ExtendUserSubscriptionCommandHandler(
            IPublishEndpoint publishEndpoint,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<ExtendUserSubscriptionCommand> HandleAsync(
            ExtendUserSubscriptionCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _queryProcessor.ExecuteAsync(new GetUserQuery(command.Email), cancellationToken);
            await _identityClient.ExtendUserSubscriptionAsync(user.SubjectId, cancellationToken);
            var newExpiryDate = DateTime.UtcNow > command.CurrentExpiryDateTimeUtc
                ? DateTime.UtcNow.AddYears(1)
                : command.CurrentExpiryDateTimeUtc.AddYears(1);
            await PublishEventAsync(user, newExpiryDate, cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private Task PublishEventAsync(
            UserReadModel model,
            DateTime expiryDateTimeUtc,
            CancellationToken cancellationToken)
        {
            return _publishEndpoint.Publish(
                new UserSubscriptionExtended(
                    model.FirstName,
                    model.LastName,
                    model.Email,
                    model.Phone,
                    expiryDateTimeUtc), cancellationToken);
        }
    }
}
