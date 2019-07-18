namespace TestOkur.WebApi.Application.Score
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class DeleteUserScoreFormulasCommandHandler
		: RequestHandlerAsync<DeleteUserScoreFormulasCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public DeleteUserScoreFormulasCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Populate(1)]
		[ClearCache(2)]
		public override async Task<DeleteUserScoreFormulasCommand> HandleAsync(
			DeleteUserScoreFormulasCommand command,
			CancellationToken cancellationToken = default)
		{
			var formulas = _dbContext.ScoreFormulas
				.Where(s => EF.Property<int>(s, "CreatedBy") == command.UserId &&
				            EF.Property<int>(s, "CreatedBy") != default)
				.ToList();

			if (formulas.Any())
			{
				_dbContext.ScoreFormulas.RemoveRange(formulas);
				await _dbContext.SaveChangesAsync(cancellationToken);
			}

			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
