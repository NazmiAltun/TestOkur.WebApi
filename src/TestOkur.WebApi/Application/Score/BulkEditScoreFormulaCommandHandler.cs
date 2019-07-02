namespace TestOkur.WebApi.Application.Score
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.Infrastructure.Cqrs;

	public sealed class BulkEditScoreFormulaCommandHandler
		: RequestHandlerAsync<BulkEditScoreFormulaCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public BulkEditScoreFormulaCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<BulkEditScoreFormulaCommand> HandleAsync(
			BulkEditScoreFormulaCommand command,
			CancellationToken cancellationToken = default)
		{
			foreach (var subCommand in command.Commands)
			{
				var formula = await GetAsync(
					subCommand.ScoreFormulaId,
					command.UserId,
					cancellationToken);

				formula.Update(
					subCommand.BasePoint,
					subCommand.Coefficients);
			}

			await _dbContext.SaveChangesAsync(cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<ScoreFormula> GetAsync(
			int id,
			int userId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.ScoreFormulas
				.Include(s => s.Coefficients)
				.FirstAsync(
					f => f.Id == id &&
					     EF.Property<int>(f, "CreatedBy") == userId,
					cancellationToken);
		}
	}
}
