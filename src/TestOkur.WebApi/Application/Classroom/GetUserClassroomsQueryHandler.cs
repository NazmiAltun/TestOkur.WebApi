namespace TestOkur.WebApi.Application.Classroom
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Configuration;

    public sealed class GetUserClassroomsQueryHandler
        : QueryHandlerAsync<GetUserClassroomsQuery, IReadOnlyCollection<ClassroomReadModel>>
    {
        private readonly string _connectionString;

        public GetUserClassroomsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(2)]
        [ResultCaching(3)]
        public override async Task<IReadOnlyCollection<ClassroomReadModel>> ExecuteAsync(
            GetUserClassroomsQuery query,
            CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM classrooms " +
                               "WHERE created_by=@userId " +
                               "ORDER BY grade_value,name_value";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<ClassroomReadModel>(
                    sql,
                    new { userId = query.UserId })).ToList();
            }
        }
    }
}
