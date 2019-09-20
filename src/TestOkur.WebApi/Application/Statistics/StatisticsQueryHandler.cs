namespace TestOkur.WebApi.Application.Statistics
{
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.WebApi.Configuration;

    public sealed class StatisticsQueryHandler : QueryHandlerAsync<StatisticsQuery, StatisticsReadModel>
    {
        private readonly string _connectionString;

        public StatisticsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(1)]
        public override async Task<StatisticsReadModel> ExecuteAsync(StatisticsQuery query, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT * FROM 
                                (SELECT COUNT(*) AS total_eschool_student_count FROM students WHERE source='ESchool') Q1,
                                (SELECT COUNT(*) AS total_bulk_student_count FROM students WHERE source IN('Bulk','Migration')) Q2,
                                (SELECT COUNT(*) AS total_single_entry_student_count FROM students WHERE source='Single') Q3,
                                (SELECT COUNT(*) AS total_scanned_student_form_count_by_camera FROM exam_scan_sessions WHERE by_camera=true) Q4,
                                (SELECT COUNT(*) AS total_scanned_student_form_count_by_file FROM exam_scan_sessions WHERE by_file=true) Q5,
                                (SELECT COUNT(*) AS total_exam_count FROM exams) Q6,
                                (SELECT COUNT(*) AS today_eschool_student_count FROM students WHERE source='ESchool' AND created_on_utc > timezone('utc',  now()::date)) Q7,
                                (SELECT COUNT(*) AS today_bulk_student_count FROM students WHERE source IN('Bulk','Migration') AND created_on_utc > timezone('utc',  now()::date)) Q8,
                                (SELECT COUNT(*) AS today_single_entry_student_count FROM students WHERE source='Single' AND created_on_utc > timezone('utc',  now()::date)) Q9,
                                (SELECT COUNT(*) AS today_scanned_student_form_count_by_camera FROM exam_scan_sessions WHERE by_camera=true AND created_on_utc > timezone('utc',  now()::date)) Q10,
                                (SELECT COUNT(*) AS today_scanned_student_form_count_by_file FROM exam_scan_sessions WHERE by_file=true AND created_on_utc > timezone('utc',  now()::date)) Q11,
                                (SELECT COUNT(*) AS today_exam_count FROM exams WHERE created_on_utc > timezone('utc',  now()::date)) Q12
                                ";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QuerySingleAsync<StatisticsReadModel>(sql);
            }
        }
    }
}
