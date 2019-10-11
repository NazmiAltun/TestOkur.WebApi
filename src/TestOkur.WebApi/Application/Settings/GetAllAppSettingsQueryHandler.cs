namespace TestOkur.WebApi.Application.Settings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Configuration;

    public sealed class GetAllAppSettingsQueryHandler : QueryHandlerAsync<GetAllAppSettingsQuery, IReadOnlyCollection<AppSettingReadModel>>
    {
        private readonly string _connectionString;

        public GetAllAppSettingsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(1)]
        [ResultCaching(2)]
        public override async Task<IReadOnlyCollection<AppSettingReadModel>> ExecuteAsync(GetAllAppSettingsQuery query, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT id,name_value as Name,value,
                                 comment,created_on_utc,updated_on_utc FROM appsettings";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<AppSettingReadModel>(sql)).ToList();
            }
        }
    }
}
