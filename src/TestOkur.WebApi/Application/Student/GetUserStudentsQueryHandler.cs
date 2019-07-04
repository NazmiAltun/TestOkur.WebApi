namespace TestOkur.WebApi.Application.Student
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
	using TestOkur.WebApi.Application.Contact;
	using TestOkur.WebApi.Configuration;

	public sealed class GetUserStudentsQueryHandler
		: QueryHandlerAsync<GetUserStudentsQuery, IReadOnlyCollection<StudentReadModel>>
	{
		private const string Sql = @"SELECT s.*,cr.name_value, cr.grade_value, c.phone_value as phone,
								s.classroom_id, c.contact_type_id as contact_type  FROM students s
								INNER JOIN classrooms cr on cr.Id=s.classroom_id
								LEFT JOIN contacts c ON c.student_id=s.Id
								WHERE s.created_by=@userId
								ORDER BY s.student_number_value
                                ";

		private readonly string _connectionString;

		public GetUserStudentsQueryHandler(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		[ResultCaching(3)]
		public override async Task<IReadOnlyCollection<StudentReadModel>> ExecuteAsync(
			GetUserStudentsQuery query,
			CancellationToken cancellationToken = default)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				var dictionary = new Dictionary<int, StudentReadModel>();

				return (await connection.QueryAsync<StudentReadModel, ContactReadModel, StudentReadModel>(
						Sql,
						(student, contact) =>
						{
							if (!dictionary.TryGetValue(student.Id, out var studentEntry))
							{
								studentEntry = student;
								dictionary.Add(studentEntry.Id, studentEntry);
							}

							AddContact(contact, studentEntry);
							return studentEntry;
						},
						new { query.UserId },
						splitOn: "phone"))
					.Distinct()
					.ToList();
			}
		}

		private void AddContact(ContactReadModel contact, StudentReadModel studentEntry)
		{
			if (!string.IsNullOrEmpty(contact?.Phone))
			{
				studentEntry.Contacts.Add(contact);
			}
		}
	}
}
