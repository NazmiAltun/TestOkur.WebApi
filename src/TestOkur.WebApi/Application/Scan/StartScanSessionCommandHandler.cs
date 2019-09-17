namespace TestOkur.WebApi.Application.Scan
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Paramore.Brighter;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Infrastructure.Cqrs;
    using Exam = TestOkur.Domain.Model.ExamModel.Exam;

    public class StartScanSessionCommandHandler
		: RequestHandlerAsync<StartScanSessionCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public StartScanSessionCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		public override async Task<StartScanSessionCommand> HandleAsync(
			StartScanSessionCommand command,
			CancellationToken cancellationToken = default)
		{
			var exam = await GetExamAsync(command.ExamId, cancellationToken);
			var session = new ExamScanSession(
				exam,
				command.Id,
				command.ByCamera,
				command.ByFile,
				command.Source);
			session.Start();
			_dbContext.ExamScanSessions.Add(session);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<Exam> GetExamAsync(
			int examId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.Exams
				.FirstOrDefaultAsync(
					l => l.Id == examId, cancellationToken);
		}
	}
}
