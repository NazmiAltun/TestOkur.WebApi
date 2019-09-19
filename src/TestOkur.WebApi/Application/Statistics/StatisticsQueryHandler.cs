namespace TestOkur.WebApi.Application.Statistics
{
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Configuration;

    public sealed class StatisticsQueryHandler  : QueryHandlerAsync<StatisticsQuery, StatisticsReadModel>
    {
        private readonly string _connectionString;

        public StatisticsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(1)]
        public override Task<StatisticsReadModel> ExecuteAsync(StatisticsQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
