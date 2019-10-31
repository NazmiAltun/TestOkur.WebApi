namespace TestOkur.WebApi.Application.User.Commands
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;
    using User = TestOkur.Domain.Model.UserModel.User;

    public sealed class UpdateUserByAdminCommandHandler
        : RequestHandlerAsync<UpdateUserByAdminCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly IIdentityClient _identityClient;

        public UpdateUserByAdminCommandHandler(
            IIdentityClient identityClient, IApplicationDbContextFactory dbContextFactory)
        {
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));
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
            await _identityClient.UpdateUserAsync(command.ToIdentityUpdateUserModel(), cancellationToken);
        }

        private async Task UpdateWebApiUserAsync(UpdateUserByAdminCommand command, CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.Create(command.UserId);
            var user = await GetUserAsync(dbContext, command.UpdatedUserId, cancellationToken);
            user.Update(command.Email, command.FirstName, command.LastName, command.CityId, command.DistrictId, command.SchoolName, command.MobilePhone, command.Referrer, command.Notes);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<User> GetUserAsync(ApplicationDbContext dbContext, int userId, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Id == userId,
                    cancellationToken);

            return user ?? throw new ArgumentException("User does not exist", nameof(userId));
        }
    }
}
