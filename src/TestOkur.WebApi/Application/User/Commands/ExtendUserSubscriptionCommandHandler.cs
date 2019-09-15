namespace TestOkur.WebApi.Application.User.Commands
{
    using MassTransit;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Application.User.Services;

    public sealed class ExtendUserSubscriptionCommandHandler : RequestHandlerAsync<ExtendUserSubscriptionCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _identityService;
        private readonly IQueryProcessor _queryProcessor;

        public ExtendUserSubscriptionCommandHandler(
            IPublishEndpoint publishEndpoint,
            IIdentityService identityService,
            IQueryProcessor queryProcessor)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
        }

        public override async Task<ExtendUserSubscriptionCommand> HandleAsync(
            ExtendUserSubscriptionCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _queryProcessor.ExecuteAsync(new GetUserByEmailQuery(command.Email), cancellationToken);
            await _identityService.ExtendUserSubscriptionAsync(user.SubjectId, cancellationToken);
            await PublishEventAsync(user, command.CurrentExpiryDateTimeUtc.AddYears(1), cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(
            UserReadModel model,
            DateTime expiryDateTimeUtc,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
                new UserSubscriptionExtended(
                    model.FirstName,
                    model.LastName,
                    model.Email,
                    model.Phone,
                    expiryDateTimeUtc), cancellationToken);
        }
    }
}
