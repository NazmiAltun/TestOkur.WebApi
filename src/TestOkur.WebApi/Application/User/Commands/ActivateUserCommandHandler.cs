namespace TestOkur.WebApi.Application.User.Commands
{
    using MassTransit;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class ActivateUserCommandHandler : RequestHandlerAsync<ActivateUserCommand>
    {
        private readonly IBus _bus;
        private readonly IIdentityClient _identityClient;
        private readonly IQueryProcessor _queryProcessor;

        public ActivateUserCommandHandler(
            IBus bus,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
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
            await _bus.Publish(
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
