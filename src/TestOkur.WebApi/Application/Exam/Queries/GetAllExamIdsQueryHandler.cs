namespace TestOkur.WebApi.Application.Exam.Queries
{
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Configuration;

    public sealed class GetAllExamIdsQueryHandler
        : QueryHandlerAsync<GetAllExamIdsQuery, IEnumerable<int>>
    {
        private readonly string _connectionString;

        public GetAllExamIdsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        public override async Task<IEnumerable<int>> ExecuteAsync(
            GetAllExamIdsQuery query,
            CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<int>("SELECT id FROM exams")).ToList();
            }
        }
    }
}
