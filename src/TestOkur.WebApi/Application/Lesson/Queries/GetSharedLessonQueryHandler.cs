namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Configuration;

    public sealed class GetSharedLessonQueryHandler : QueryHandlerAsync<GetSharedLessonQuery, IReadOnlyCollection<LessonReadModel>>
    {
        private readonly string _connectionString;

        public GetSharedLessonQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(2)]
        public override async Task<IReadOnlyCollection<LessonReadModel>> ExecuteAsync(GetSharedLessonQuery query, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT id,name_value AS Name, 1 as shared FROM lessons
							   WHERE created_by=0
	                           ORDER BY name_value";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection
                    .QueryAsync<LessonReadModel>(sql))
                    .ToList();
            }
        }
    }
}
