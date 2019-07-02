namespace TestOkur.Report.Infrastructure
{
	using System.Threading.Tasks;

	using TestOkur.Infrastructure.Mvc;
	using TestOkur.Report.Configuration;

	public class RequestResponseMongodbLogger : IRequestResponseLogger
	{
		private readonly TestOkurContext _context;

		public RequestResponseMongodbLogger(ReportConfiguration configuration)
		{
			_context = new TestOkurContext(configuration);
		}

		public async Task PersistAsync(RequestResponseLog log)
		{
			await _context.RequestResponseLogs.InsertOneAsync(log);
		}
	}
}
