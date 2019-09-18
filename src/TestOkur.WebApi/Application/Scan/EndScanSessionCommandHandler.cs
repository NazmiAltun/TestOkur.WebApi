namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;

    public class EndScanSessionCommandHandler
        : RequestHandlerAsync<EndScanSessionCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public EndScanSessionCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        public override async Task<EndScanSessionCommand> HandleAsync(
            EndScanSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var session = await dbContext.ExamScanSessions.FirstAsync(
                    e => e.ReportId == command.Id,
                    cancellationToken);
                session.End(command.ScannedStudentCount);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
