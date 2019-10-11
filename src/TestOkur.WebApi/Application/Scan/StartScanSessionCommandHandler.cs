namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Infrastructure.CommandsQueries;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;

    public class StartScanSessionCommandHandler
        : RequestHandlerAsync<StartScanSessionCommand>
    {
        private readonly IApplicationDbContextFactory _dbContextFactory;

        public StartScanSessionCommandHandler(IApplicationDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [Idempotent(1)]
        public override async Task<StartScanSessionCommand> HandleAsync(
            StartScanSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.Create(command.UserId))
            {
                var exam = await GetExamAsync(dbContext, command.ExamId, cancellationToken);
                var session = new ExamScanSession(
                    exam,
                    command.Id,
                    command.ByCamera,
                    command.ByFile,
                    command.Source);
                session.Start();
                dbContext.ExamScanSessions.Add(session);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return await base.HandleAsync(command, cancellationToken);
        }

        private async Task<Exam> GetExamAsync(
            ApplicationDbContext dbContext,
            int examId,
            CancellationToken cancellationToken)
        {
            return await dbContext.Exams
                .FirstOrDefaultAsync(
                    l => l.Id == examId, cancellationToken);
        }
    }
}
