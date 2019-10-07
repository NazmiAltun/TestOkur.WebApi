﻿namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core.Logging;
    using Microsoft.Extensions.Logging;
    using TestOkur.Data;
    using TestOkur.Infrastructure.Cqrs;

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

        [Idempotent(1)]
        public override async Task<EndScanSessionCommand> HandleAsync(
            EndScanSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
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
