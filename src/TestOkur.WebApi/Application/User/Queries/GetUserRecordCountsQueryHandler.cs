namespace TestOkur.WebApi.Application.User.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.WebApi.Configuration;

    public sealed class GetUserRecordCountsQueryHandler :
        QueryHandlerAsync<GetUserRecordCountsQuery, UserRecords>
    {
        private readonly string _connectionString;

        public GetUserRecordCountsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        public override Task<UserRecords> ExecuteAsync(
            GetUserRecordCountsQuery query,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT  (SELECT COUNT(*) from classrooms WHERE created_by=@userId) as classroom_count,
  								(SELECT COUNT(*) from lessons WHERE created_by=@userId) as lesson_count,
								(SELECT COUNT(*) from students WHERE created_by=@userId) as student_count,
								(SELECT COUNT(*) from exams WHERE created_by=@userId) as exam_count
								";
            using var connection = new NpgsqlConnection(_connectionString);
            return connection.QuerySingleAsync<UserRecords>(
                sql,
                new { userId = query.UserId });
        }
    }
}
