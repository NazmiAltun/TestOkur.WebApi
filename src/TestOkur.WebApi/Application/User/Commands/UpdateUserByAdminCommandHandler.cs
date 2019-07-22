namespace TestOkur.WebApi.Application.User.Commands
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;
	using TestOkur.WebApi.Application.User.Services;
	using User = TestOkur.Domain.Model.UserModel.User;

	public sealed class UpdateUserByAdminCommandHandler
		: RequestHandlerAsync<UpdateUserByAdminCommand>
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IIdentityService _identityService;

		public UpdateUserByAdminCommandHandler(
			IIdentityService identityService,
			ApplicationDbContext dbContext)
		{
			_identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
			_dbContext = dbContext;
		}

		[Idempotent(1)]
		[ClearCache(2)]
		public override async Task<UpdateUserByAdminCommand> HandleAsync(
			UpdateUserByAdminCommand command,
			CancellationToken cancellationToken = default)
		{
			await UpdateWebApiUserAsync(command, cancellationToken);
			await UpdateIdentityUserAsync(command, cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task UpdateIdentityUserAsync(UpdateUserByAdminCommand command, CancellationToken cancellationToken)
		{
			await _identityService.UpdateUserAsync(command.ToIdentityUpdateUserModel(), cancellationToken);
		}

		private async Task UpdateWebApiUserAsync(UpdateUserByAdminCommand command, CancellationToken cancellationToken)
		{
			var user = await GetUserAsync(command.UpdatedUserId, cancellationToken);
			var city = await GetCityAsync(command.CityId, cancellationToken);
			var district = city.Districts.First(d => d.Id == command.DistrictId);
			user.Update(command.Email, command.FirstName, command.LastName, city, district, command.SchoolName, command.MobilePhone);
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		private async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.FirstOrDefaultAsync(
					u => u.Id == userId,
					cancellationToken);

			return user ?? throw new ArgumentException("User does not exist", nameof(userId));
		}

		private async Task<Domain.Model.CityModel.City> GetCityAsync(int cityId, CancellationToken cancellationToken)
		{
			var city = await _dbContext.Cities
				.Include(c => c.Districts)
				.FirstOrDefaultAsync(
					u => u.Id == cityId,
					cancellationToken);

			return city ?? throw new ArgumentException("City does not exist", nameof(cityId));
		}
	}
}
