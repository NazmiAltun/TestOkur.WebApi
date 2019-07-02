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
		private readonly ApplicationDbContext _dbContext;

		public EndScanSessionCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[Populate(2)]
		public override async Task<EndScanSessionCommand> HandleAsync(
			EndScanSessionCommand command,
			CancellationToken cancellationToken = default)
		{
			var session = await _dbContext.ExamScanSessions.FirstAsync(
				e => e.ReportId == command.Id,
				cancellationToken);
			session.End(command.ScannedStudentCount);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return await base.HandleAsync(command, cancellationToken);
		}
	}
}
