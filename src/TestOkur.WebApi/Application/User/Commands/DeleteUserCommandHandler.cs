namespace TestOkur.WebApi.Application.User.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.User.Services;

    public class DeleteUserCommandHandler : RequestHandlerAsync<DeleteUserCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public DeleteUserCommandHandler(IIdentityService identityService, IApplicationDbContextFactory dbContextFactory)
        {
            _identityService = identityService;
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(2)]
        public override async Task<DeleteUserCommand> HandleAsync(
            DeleteUserCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(
                    l => l.Id == command.DeleteUserId, cancellationToken);

                if (user != null)
                {
                    await _identityService.DeleteUserAsync(user.SubjectId, cancellationToken);
                    dbContext.Remove(user);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
