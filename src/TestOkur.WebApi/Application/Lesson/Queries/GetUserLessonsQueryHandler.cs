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

    public sealed class GetUserLessonsQueryHandler : QueryHandlerAsync<GetUserLessonsQuery, IReadOnlyCollection<LessonReadModel>>
    {
        private readonly string _connectionString;

        public GetUserLessonsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(1)]
        public override async Task<IReadOnlyCollection<LessonReadModel>> ExecuteAsync(
            GetUserLessonsQuery query,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT id,name_value AS Name, 1 as shared FROM lessons
								WHERE created_by=0
								UNION
								SELECT id,name_value AS Name, 0 as shared FROM lessons
								WHERE created_by=@userId
								ORDER BY Name";

            await using var connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<LessonReadModel>(
                sql,
                new { userId = query.UserId })).ToList();
        }
    }
}
