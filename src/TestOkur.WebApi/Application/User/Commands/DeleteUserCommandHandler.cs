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
		private readonly ApplicationDbContext _dbContext;

		public DeleteUserCommandHandler(IIdentityService identityService, ApplicationDbContext dbContext)
		{
			_identityService = identityService;
			_dbContext = dbContext;
		}

		[Idempotent(1)]
		[ClearCache(2)]
		public override async Task<DeleteUserCommand> HandleAsync(
			DeleteUserCommand command,
			CancellationToken cancellationToken = default)
		{
			var user = await GetUserAsync(command.DeleteUserId, cancellationToken);

			if (user != null)
			{
				await _identityService.DeleteUserAsync(user.SubjectId, cancellationToken);
				_dbContext.Remove(user);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Domain.Model.UserModel.User> GetUserAsync(
			int id,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(
				l => l.Id == id, cancellationToken);
		}
	}
}
