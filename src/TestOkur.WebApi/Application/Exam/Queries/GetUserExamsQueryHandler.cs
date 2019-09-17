namespace TestOkur.WebApi.Application.Exam.Queries
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

    public sealed class GetUserExamsQueryHandler
		: QueryHandlerAsync<GetUserExamsQuery, IReadOnlyCollection<ExamReadModel>>
	{
		private readonly string _connectionString;

		public GetUserExamsQueryHandler(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		[ResultCaching(3)]
		public override async Task<IReadOnlyCollection<ExamReadModel>> ExecuteAsync(
			GetUserExamsQuery query,
			CancellationToken cancellationToken = default)
		{
			const string sql = @"SELECT 
								e.id,
								e.answer_form_format_id,
								e.name_value as name,
								e.exam_type_id,
								e.incorrect_elimination_rate_value as incorrect_elimination_rate,
								e.notes,
								e.exam_booklet_type_id,
								e.exam_date,
								e.applicable_form_type_code,
								et.name_value as exam_type_name,
								COALESCE(l.name_value,fls.lesson_name) as lesson_name,
								l.id as lesson_id
								FROM exams e
								INNER JOIN exam_types et ON et.id=e.exam_type_id
								LEFT JOIN lessons l ON l.id=e.lesson_id
								LEFT JOIN(
									SELECT oft.code,  string_agg(le.name_value, ' - ') as lesson_name
								FROM optical_form_types oft 
								INNER JOIN form_lesson_sections fls ON fls.optical_form_type_id=oft.id
								INNER JOIN lessons le ON le.id=fls.lesson_id
								GROUP BY oft.code
								)fls ON e.applicable_form_type_code=fls.code
								WHERE e.created_by=@userId
								ORDER BY e.created_on_utc DESC";

			using (var connection = new NpgsqlConnection(_connectionString))
			{
				return (await connection.QueryAsync<ExamReadModel>(
					sql,
					new { userId = query.UserId })).ToList();
			}
		}
	}
}
