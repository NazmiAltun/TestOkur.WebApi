namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Common.Configuration;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Application.User.Services;

    public sealed class SendResetPasswordLinkCommandHandler : RequestHandlerAsync<SendResetPasswordLinkCommand>
	{
		private readonly IQueryProcessor _queryProcessor;
		private readonly ICaptchaService _captchaService;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IIdentityService _identityService;
		private readonly OAuthConfiguration _oAuthConfiguration;

		public SendResetPasswordLinkCommandHandler(
			ICaptchaService captchaService,
			IPublishEndpoint publishEndpoint,
			IIdentityService identityService,
			IQueryProcessor queryProcessor,
			OAuthConfiguration oAuthConfiguration)
		{
			_captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
			_queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
			_oAuthConfiguration = oAuthConfiguration;
		}

		[Idempotent(1)]
		public override async Task<SendResetPasswordLinkCommand> HandleAsync(
			SendResetPasswordLinkCommand command,
			CancellationToken cancellationToken = default)
		{
			ValidateCaptcha(command);
			var token = await _identityService.GeneratePasswordResetTokenAsync(command.Email, cancellationToken);
			await PublishEventAsync(command.Email, token, cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task PublishEventAsync(
			string email,
			string token,
			CancellationToken cancellationToken)
		{
			var user = await _queryProcessor.ExecuteAsync(
				new GetUserByEmailQuery(email),
				cancellationToken);
			var url = $"{_oAuthConfiguration.Authority}account/reset-password?token={token}&email={email}";

			await _publishEndpoint.Publish(
				new ResetPasswordTokenGenerated(url, email, user.FirstName, user.LastName),
				cancellationToken);
		}

		private void ValidateCaptcha(SendResetPasswordLinkCommand command)
		{
			if (!_captchaService.Validate(command.CaptchaId, command.CaptchaCode))
			{
				throw new ValidationException(ErrorCodes.InvalidCaptcha);
			}
		}
	}
}
