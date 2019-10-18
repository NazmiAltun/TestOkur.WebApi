namespace TestOkur.WebApi.Application.Score
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public sealed class DeleteUserScoreFormulasCommandHandler
        : RequestHandlerAsync<DeleteUserScoreFormulasCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public DeleteUserScoreFormulasCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [ClearCache(2)]
        public override async Task<DeleteUserScoreFormulasCommand> HandleAsync(
            DeleteUserScoreFormulasCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var formulas = dbContext.ScoreFormulas
                    .Where(s => EF.Property<int>(s, "CreatedBy") == command.UserId &&
                                EF.Property<int>(s, "CreatedBy") != default)
                    .ToList();

                if (formulas.Any())
                {
                    dbContext.ScoreFormulas.RemoveRange(formulas);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
