namespace TestOkur.WebApi.Application.Statistics
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.WebApi.Configuration;

	//TODO:IMplement
    public sealed class DailyReportStatisticsQueryHandler
        : QueryHandlerAsync<DailyReportStatisticsQuery, DailyReportStatisticsReadModel>
    {
        private readonly string _connectionString;

        public DailyReportStatisticsQueryHandler(ApplicationConfiguration configurationOptions)
        {
			_connectionString = configurationOptions.Postgres;
		}

        [QueryLogging(1)]
        public override Task<DailyReportStatisticsReadModel> ExecuteAsync(DailyReportStatisticsQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
