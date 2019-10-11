﻿namespace TestOkur.WebApi.Application.User.Commands
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Services;
    using User = TestOkur.Domain.Model.UserModel.User;

    public sealed class UpdateUserByAdminCommandHandler
        : RequestHandlerAsync<UpdateUserByAdminCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IIdentityService _identityService;

        public UpdateUserByAdminCommandHandler(
            IIdentityService identityService, IApplicationDbContextFactory dbContextFactory)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _dbContextFactory = dbContextFactory;
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
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var user = await GetUserAsync(dbContext, command.UpdatedUserId, cancellationToken);
                var city = await GetCityAsync(dbContext, command.CityId, cancellationToken);
                var district = city.Districts.First(d => d.Id == command.DistrictId);
                user.Update(command.Email, command.FirstName, command.LastName, city, district, command.SchoolName, command.MobilePhone, command.Notes);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task<User> GetUserAsync(ApplicationDbContext dbContext, int userId, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Id == userId,
                    cancellationToken);

            return user ?? throw new ArgumentException("User does not exist", nameof(userId));
        }

        private async Task<Domain.Model.CityModel.City> GetCityAsync(ApplicationDbContext dbContext, int cityId, CancellationToken cancellationToken)
        {
            var city = await dbContext.Cities
                .Include(c => c.Districts)
                .FirstOrDefaultAsync(
                    u => u.Id == cityId,
                    cancellationToken);

            return city ?? throw new ArgumentException("City does not exist", nameof(cityId));
        }
    }
}
