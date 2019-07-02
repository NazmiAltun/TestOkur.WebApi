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
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using TestOkur.WebApi.Application.Captcha;
	using TestOkur.WebApi.Application.User.Events;
	using TestOkur.WebApi.Application.User.Services;

	public sealed class CreateUserCommandHandler : RequestHandlerAsync<CreateUserCommand>
	{
		private readonly ICaptchaService _captchaService;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IIdentityService _identityService;
		private readonly ApplicationDbContext _dbContext;

		public CreateUserCommandHandler(
			ICaptchaService captchaService,
			ApplicationDbContext dbContext,
			IPublishEndpoint publishEndpoint,
			IIdentityService identityService)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_captchaService = captchaService ?? throw new ArgumentNullException(nameof(captchaService));
			_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
			_identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
		}

		[Idempotent(1)]
		[ClearCache(2)]
		public override async Task<CreateUserCommand> HandleAsync(
			CreateUserCommand command,
			CancellationToken cancellationToken = default)
		{
			ValidateCaptcha(command);
			await EnsureUserDoesNotExistAsync(command, cancellationToken);
			await SaveToDatabaseAsync(command, cancellationToken);
			await RegisterUserAsync(command, cancellationToken);
			await PublishEventAsync(command, cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task RegisterUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
		{
			var licenseType = await _dbContext.LicenseTypes
				.SingleAsync(l => l.Id == command.LicenseTypeId, cancellationToken);
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

		private async Task SaveToDatabaseAsync(CreateUserCommand command, CancellationToken cancellationToken)
		{
			var city = await _dbContext.Cities
				.Include(c => c.Districts)
				.FirstAsync(c => c.Id == command.CityId, cancellationToken);
			var district = city.Districts.First(d => d.Id == command.DistrictId);

			_dbContext.Users.Add(command.ToDomainModel(city, district));
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		private async Task EnsureUserDoesNotExistAsync(
			CreateUserCommand command,
			CancellationToken cancellationToken = default)
		{
			if (await _dbContext.Users.AnyAsync(
				l => l.Email.Value == command.Email,
				cancellationToken))
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
