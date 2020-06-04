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
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class SendResetPasswordLinkCommandHandler : RequestHandlerAsync<SendResetPasswordLinkCommand>
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICaptchaService _captchaService;
        private readonly IBus _bus;
        private readonly IIdentityClient _identityClient;
        private readonly OAuthConfiguration _oAuthConfiguration;

        public SendResetPasswordLinkCommandHandler(
            ICaptchaService captchaService,
            IBus bus,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor,
            OAuthConfiguration oAuthConfiguration)
        {
            _captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
            _queryProcessor = queryProcessor ?? throw new ArgumentNullException(nameof(queryProcessor));
            _oAuthConfiguration = oAuthConfiguration;
        }

        [Idempotent(1)]
        public override async Task<SendResetPasswordLinkCommand> HandleAsync(
            SendResetPasswordLinkCommand command,
            CancellationToken cancellationToken = default)
        {
            await ValidateCaptchaAsync(command);
            var token = await _identityClient.GeneratePasswordResetTokenAsync(command.Email, cancellationToken);
            await PublishEventAsync(command.Email, token, cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task PublishEventAsync(
            string email,
            string token,
            CancellationToken cancellationToken)
        {
            var user = await _queryProcessor.ExecuteAsync(
                new GetUserQuery(email),
                cancellationToken);
            var url = $"{_oAuthConfiguration.Authority}account/reset-password?token={token}&email={email}";

            await _bus.Publish(
                new ResetPasswordTokenGenerated(url, email, user.FirstName, user.LastName),
                cancellationToken);
        }

        private async Task ValidateCaptchaAsync(SendResetPasswordLinkCommand command)
        {
            if (!await _captchaService
                .ValidateAsync(command.CaptchaId, command.CaptchaCode))
            {
                throw new ValidationException(ErrorCodes.InvalidCaptcha);
            }
        }
    }
}
