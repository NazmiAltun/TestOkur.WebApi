namespace TestOkur.WebApi.Application.User.Commands
{
    using MassTransit;
    using Paramore.Brighter;
    using Paramore.Darker;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.User.Clients;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;

    public sealed class CreateUserCommandHandler : RequestHandlerAsync<CreateUserCommand>
    {
        private readonly ICaptchaService _captchaService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityClient _identityClient;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly ISabitClient _sabitClient;

        public CreateUserCommandHandler(
            ICaptchaService captchaService,
            IPublishEndpoint publishEndpoint,
            IIdentityClient identityClient,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory dbContextFactory,
            ISabitClient sabitClient)
        {
            _captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
            _queryProcessor = queryProcessor;
            _dbContextFactory = dbContextFactory;
            _sabitClient = sabitClient;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<CreateUserCommand> HandleAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            await ValidateCaptchaAsync(command);

            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                await EnsureUserDoesNotExistAsync(command, cancellationToken);
                await SaveToDatabaseAsync(dbContext, command, cancellationToken);
            }

            await RegisterUserAsync(command, cancellationToken);
            await PublishEventAsync(command, cancellationToken);

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task RegisterUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var licenseType = (await _sabitClient.GetLicenseTypesAsync())
                .First(l => l.Id == command.LicenseTypeId);
            var model = new CreateCustomerUserModel()
            {
                Email = command.Email,
                Id = command.Id.ToString(),
                CanScan = licenseType.CanScan,
                LicenseTypeId = command.LicenseTypeId,
                MaxAllowedDeviceCount = licenseType.MaxAllowedDeviceCount,
                MaxAllowedStudentCount = licenseType.MaxAllowedRecordCount,
                Password = command.Password,
            };
            await _identityClient.RegisterUserAsync(model, cancellationToken);
        }

        private async Task SaveToDatabaseAsync(
            ApplicationDbContext dbContext,
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            dbContext.Users.Add(command.ToDomainModel());
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureUserDoesNotExistAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor.ExecuteAsync(GetAllUsersQuery.Default, cancellationToken);
            if (users.Any(l => l.Email == command.Email))
            {
                throw new ValidationException(ErrorCodes.UserAlreadyExists);
            }
        }

        private Task PublishEventAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            return _publishEndpoint.Publish(
                 new NewUserRegistered(
                     command.Email,
                     command.RegistrarFullName,
                     command.RegistrarPhone,
                     command.UserFirstName,
                     command.UserLastName,
                     command.SchoolName,
                     command.UserPhone,
                     command.LicenseTypeName,
                     command.DistrictName,
                     command.CityName,
                     command.Password,
                     command.Referrer), cancellationToken);
        }

        private async Task ValidateCaptchaAsync(CreateUserCommand command)
        {
            if (!await _captchaService
                .ValidateAsync(command.CaptchaId, command.CaptchaCode))
            {
                throw new ValidationException(ErrorCodes.InvalidCaptcha);
            }
        }
    }
}
