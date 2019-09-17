namespace TestOkur.WebApi.Application.Score
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ScoreModel;
    using TestOkur.Infrastructure.Cqrs;

    public sealed class CloneScoreFormulaCommandHandler
		: RequestHandlerAsync<CloneScoreFormulaCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public CloneScoreFormulaCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ??
			             throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[ClearCache(3)]
		public override async Task<CloneScoreFormulaCommand> HandleAsync(
			CloneScoreFormulaCommand command,
			CancellationToken cancellationToken = default)
		{
			var scoreFormulas = await _dbContext.ScoreFormulas
				.Include(s => s.FormulaType)
				.Include(s => s.Coefficients)
				.ThenInclude(c => c.ExamLessonSection)
				.ThenInclude(e => e.Lesson)
				.Where(s => EF.Property<int>(s, "CreatedBy") == default)
				.ToListAsync(cancellationToken);

			var list = new List<ScoreFormula>();

			foreach (var formula in scoreFormulas)
			{
				list.Add(new ScoreFormula(formula));
			}

			_dbContext.ScoreFormulas.AddRange(list);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
