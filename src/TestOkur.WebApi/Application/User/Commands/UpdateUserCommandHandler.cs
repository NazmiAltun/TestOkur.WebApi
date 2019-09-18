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
    using City = TestOkur.Domain.Model.CityModel.City;
    using User = TestOkur.Domain.Model.UserModel.User;

    public sealed class UpdateUserCommandHandler : RequestHandlerAsync<UpdateUserCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public UpdateUserCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<UpdateUserCommand> HandleAsync(
            UpdateUserCommand command,
            CancellationToken cancellationToken = default)
        {
            await UpdateAsync(command, command.UserId, cancellationToken);
            return await base.HandleAsync(command, cancellationToken);
        }

        public async Task UpdateAsync(UpdateUserCommand command, int userId, CancellationToken cancellationToken)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var user = await GetUserAsync(dbContext, userId, cancellationToken);

                if (user.Email == "demo@testokur.com")
                {
                    return;
                }

                var city = await GetCityAsync(dbContext, command.CityId, cancellationToken);
                var district = city.Districts.First(d => d.Id == command.DistrictId);
                user.Update(city, district, command.SchoolName, command.MobilePhone);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task<User> GetUserAsync(
            ApplicationDbContext dbContext,
            int userId,
            CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Id == userId,
                    cancellationToken);

            return user ?? throw new ArgumentException("User does not exist", nameof(userId));
        }

        private async Task<City> GetCityAsync(
            ApplicationDbContext dbContext,
            int cityId,
            CancellationToken cancellationToken)
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
