namespace TestOkur.WebApi.Application.LicenseType
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

	public sealed class GetAllLicenseTypesQueryHandler : QueryHandlerAsync<GetAllLicenseTypesQuery, IReadOnlyCollection<LicenseTypeReadModel>>
    {
        private readonly string _connectionString;

        public GetAllLicenseTypesQueryHandler(ApplicationConfiguration configurationOptions)
        {
	        _connectionString = configurationOptions.Postgres;
		}

        [QueryLogging(1)]
        [ResultCaching(2)]
        public override async Task<IReadOnlyCollection<LicenseTypeReadModel>> ExecuteAsync(GetAllLicenseTypesQuery query, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT id,name_value as Name,max_allowed_device_count,
                                 max_allowed_record_count,can_scan FROM license_types";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<LicenseTypeReadModel>(sql)).ToList();
            }
        }
    }
}
