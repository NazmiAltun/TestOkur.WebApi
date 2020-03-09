namespace TestOkur.WebApi.Application.Statistics
{
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Application.Exam.Queries;
    using TestOkur.WebApi.Configuration;

    public sealed class StatisticsQueryHandler : QueryHandlerAsync<StatisticsQuery, StatisticsReadModel>
    {
        private readonly string _connectionString;

        public StatisticsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        public override async Task<StatisticsReadModel> ExecuteAsync(StatisticsQuery query, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM 
                                (SELECT COUNT(*) AS total_eschool_student_count FROM students WHERE source='ESchool') Q1,
                                (SELECT COUNT(*) AS total_bulk_student_count FROM students WHERE source IN('Bulk','Migration')) Q2,
                                (SELECT COUNT(*) AS total_single_entry_student_count FROM students WHERE source='Single') Q3,
                                (SELECT SUM(scanned_student_count) AS total_scanned_student_form_count_by_camera FROM exam_scan_sessions WHERE by_camera=true) Q4,
                                (SELECT SUM(scanned_student_count) AS total_scanned_student_form_count_by_file FROM exam_scan_sessions WHERE by_file=true) Q5,
                                (SELECT COUNT(*) AS total_exam_count FROM exams) Q6,
                                (SELECT COUNT(*) AS today_eschool_student_count FROM students WHERE source='ESchool' AND created_on_utc > timezone('utc',  now()::date)) Q7,
                                (SELECT COUNT(*) AS today_bulk_student_count FROM students WHERE source IN('Bulk','Migration') AND created_on_utc > timezone('utc',  now()::date)) Q8,
                                (SELECT COUNT(*) AS today_single_entry_student_count FROM students WHERE source='Single' AND created_on_utc > timezone('utc',  now()::date)) Q9,
                                (SELECT SUM(scanned_student_count) AS today_scanned_student_form_count_by_camera FROM exam_scan_sessions WHERE by_camera=true AND created_on_utc > timezone('utc',  now()::date)) Q10,
                                (SELECT SUM(scanned_student_count) AS today_scanned_student_form_count_by_file FROM exam_scan_sessions WHERE by_file=true AND created_on_utc > timezone('utc',  now()::date)) Q11,
                                (SELECT COUNT(*) AS today_exam_count FROM exams WHERE created_on_utc > timezone('utc',  now()::date)) Q12
                                ";
            const string sharedExamsSql = @"SELECT 
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
								l.id as lesson_id,
                                e.shared
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
								WHERE e.shared=true
								ORDER BY e.shared DESC, e.created_on_utc DESC";

            await using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.QuerySingleAsync<StatisticsReadModel>(sql);
            result.SharedExams = await connection.QueryAsync<ExamReadModel>(sharedExamsSql);

            return result;
        }
    }
}
