namespace TestOkur.WebApi.Application.Score
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ScoreModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class CloneScoreFormulaCommandHandler
        : RequestHandlerAsync<CloneScoreFormulaCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public CloneScoreFormulaCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<CloneScoreFormulaCommand> HandleAsync(
            CloneScoreFormulaCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var scoreFormulas = await dbContext.ScoreFormulas
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

                dbContext.ScoreFormulas.AddRange(list);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
