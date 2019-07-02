namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;

	public class AddSmsCreditsCommandHandler : RequestHandlerAsync<AddSmsCreditsCommand>
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public AddSmsCreditsCommandHandler(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		[Idempotent(1)]
		[ClearCache(2)]
		public override async Task<AddSmsCreditsCommand> HandleAsync(
			AddSmsCreditsCommand command,
			CancellationToken cancellationToken = default)
		{
			var user = await _applicationDbContext.Users
				.FirstAsync(
					u => u.Id == command.UserId,
					cancellationToken);
			user.AddSmsBalance(command.Amount);
			await _applicationDbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
