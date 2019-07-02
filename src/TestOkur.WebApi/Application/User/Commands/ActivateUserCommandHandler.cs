namespace TestOkur.WebApi.Application.User.Commands
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using MassTransit;
	using Paramore.Brighter;
	using Paramore.Darker;
	using TestOkur.WebApi.Application.User.Events;
	using TestOkur.WebApi.Application.User.Queries;
	using TestOkur.WebApi.Application.User.Services;

	public sealed class ActivateUserCommandHandler : RequestHandlerAsync<ActivateUserCommand>
	{
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IIdentityService _identityService;
		private readonly IQueryProcessor _queryProcessor;

		public ActivateUserCommandHandler(
			IPublishEndpoint publishEndpoint,
			IIdentityService identityService,
			IQueryProcessor queryProcessor)
		{
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
		}

		public override async Task<ActivateUserCommand> HandleAsync(
			ActivateUserCommand command,
			CancellationToken cancellationToken = default)
		{
			var user = await _queryProcessor.ExecuteAsync(new GetUserByEmailQuery(command.Email), cancellationToken);

			await _identityService.ActivateUserAsync(command.Email, cancellationToken);
			await PublishEventAsync(user, cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(
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
