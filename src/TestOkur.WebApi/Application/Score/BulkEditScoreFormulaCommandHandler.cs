namespace TestOkur.WebApi.Application.Score
{
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ScoreModel;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class BulkEditScoreFormulaCommandHandler
        : RequestHandlerAsync<BulkEditScoreFormulaCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public BulkEditScoreFormulaCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        [ClearCache(3)]
        public override async Task<BulkEditScoreFormulaCommand> HandleAsync(
            BulkEditScoreFormulaCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                foreach (var subCommand in command.Commands)
                {
                    var formula = await GetAsync(
                        dbContext,
                        subCommand.ScoreFormulaId,
                        command.UserId,
                        cancellationToken);

                    formula.Update(
                        subCommand.BasePoint,
                        subCommand.Coefficients);
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private Task<ScoreFormula> GetAsync(
            ApplicationDbContext dbContext,
            int id,
            int userId,
            CancellationToken cancellationToken)
        {
            return dbContext.ScoreFormulas
                .Include(s => s.Coefficients)
                .FirstAsync(
                    f => f.Id == id &&
                         EF.Property<int>(f, "CreatedBy") == userId,
                    cancellationToken);
        }
    }
}
