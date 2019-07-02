namespace TestOkur.WebApi.Application.Contact
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

	public sealed class GetUserContactsQueryHandler
		: QueryHandlerAsync<GetUserContactsQuery, IReadOnlyCollection<ContactReadModel>>
	{
		private readonly string _connectionString;

		public GetUserContactsQueryHandler(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		[ResultCaching(3)]
		public override async Task<IReadOnlyCollection<ContactReadModel>> ExecuteAsync(
			GetUserContactsQuery query,
			CancellationToken cancellationToken = default)
		{
			const string sql = @"SELECT c.*,cr.grade_value, cr.name_value as classroom_name,
								cr.grade_value as grade, ct.id as contact_type,
								ct.name as contact_type_name FROM contacts  c
							   LEFT JOIN students s on c.student_id=s.id
							   LEFT JOIN classrooms cr on cr.id=s.classroom_id
							   INNER JOIN contact_types ct ON c.contact_type_id=ct.id
							   WHERE c.created_by=@userId
							   ORDER BY c.first_name_value , c.last_name_value";

			using (var connection = new NpgsqlConnection(_connectionString))
			{
				return (await connection.QueryAsync<ContactReadModel>(
					sql,
					new { userId = query.UserId })).ToList();
			}
		}
	}
}
