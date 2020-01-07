namespace TestOkur.WebApi.Application.Statistics
{
    using Paramore.Darker;

    public class StatisticsQuery : IQuery<StatisticsReadModel>
    {
        private StatisticsQuery()
        {
        }

        public static StatisticsQuery Default { get; } = new StatisticsQuery();
    }
}
