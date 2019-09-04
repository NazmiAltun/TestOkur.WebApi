namespace TestOkur.WebApi.Application.User.Queries
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

	public sealed class GetAllUsersQueryHandler : QueryHandlerAsync<GetAllUsersQuery, IReadOnlyCollection<UserReadModel>>
	{
		private const string Sql = @"
								SELECT
								u.id,
								subject_id,
								sms_balance,
								u.city_id,
								district_id,
								email_value as email,
								phone_value as phone,
								first_name_value as first_name,
								last_name_value as last_name,
								school_name_value as school_name,
								c.name_value as city_name,
								d.name_value as district_name,
								u.notes,
								u.registrar_phone_value as registrar_phone,
								u.registrar_full_name_value as registrar_full_name
								FROM users u
								INNER JOIN cities c ON c.id=u.city_id
								INNER JOIN districts d ON d.id=u.district_id";

		private readonly string _connectionString;

		public GetAllUsersQueryHandler(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		[ResultCaching(3)]
		public override async Task<IReadOnlyCollection<UserReadModel>> ExecuteAsync(
			GetAllUsersQuery query,
			CancellationToken cancellationToken = default)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				return (await connection.QueryAsync<UserReadModel>(Sql))
					.ToList();
			}
		}
	}
}
