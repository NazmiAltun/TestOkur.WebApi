namespace TestOkur.WebApi.Application.Scan
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Paramore.Brighter;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Infrastructure.CommandsQueries;

    public class EndScanSessionCommandHandler
        : RequestHandlerAsync<EndScanSessionCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly ILogger<EndScanSessionCommandHandler> _logger;

        public EndScanSessionCommandHandler(IApplicationDbContextFactory dbContextFactory, ILogger<EndScanSessionCommandHandler> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public override async Task<EndScanSessionCommand> HandleAsync(
            EndScanSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            await using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                try
                {
                    var session = await dbContext.ExamScanSessions.FirstAsync(
                        e => e.ReportId == command.Id,
                        cancellationToken);
                    session.End(command.ScannedStudentCount);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"EndScanSession exception : ID {command.Id} UserId {command.UserId}");
                }
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
