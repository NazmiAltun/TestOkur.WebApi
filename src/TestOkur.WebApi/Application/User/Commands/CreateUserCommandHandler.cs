namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using Paramore.Darker;
    using TestOkur.Common;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.Captcha;
    using TestOkur.WebApi.Application.LicenseType;
    using TestOkur.WebApi.Application.User.Events;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Application.User.Services;

    public sealed class CreateUserCommandHandler : RequestHandlerAsync<CreateUserCommand>
    {
        private readonly ICaptchaService _captchaService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _identityService;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public CreateUserCommandHandler(
            ICaptchaService captchaService,
            IPublishEndpoint publishEndpoint,
            IIdentityService identityService,
            IQueryProcessor queryProcessor,
            IApplicationDbContextFactory dbContextFactory)
        {
            _captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _queryProcessor = queryProcessor;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<CreateUserCommand> HandleAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            ValidateCaptcha(command);
            using (var dbContext = _dbContextFactory.Create(command.UserId))
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
            var licenseType = _queryProcessor.Execute(new GetAllLicenseTypesQuery())
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
            await _identityService.RegisterUserAsync(model, cancellationToken);
        }

        private async Task SaveToDatabaseAsync(
            ApplicationDbContext dbContext,
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            var city = await dbContext.Cities
                .Include(c => c.Districts)
                .FirstAsync(c => c.Id == command.CityId, cancellationToken);
            var district = city.Districts.First(d => d.Id == command.DistrictId);

            dbContext.Users.Add(command.ToDomainModel(city, district));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureUserDoesNotExistAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var users = await _queryProcessor.ExecuteAsync(new GetAllUsersQuery(), cancellationToken);
            if (users.Any(l => l.Email == command.Email))
            {
                throw new ValidationException(ErrorCodes.UserAlreadyExists);
            }
        }

        private async Task PublishEventAsync(
            CreateUserCommand command,
            CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(
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
                    command.CityName), cancellationToken);
        }

        private void ValidateCaptcha(CreateUserCommand command)
        {
            if (!_captchaService.Validate(command.CaptchaId, command.CaptchaCode))
            {
                throw new ValidationException(ErrorCodes.InvalidCaptcha);
            }
        }
    }
}
