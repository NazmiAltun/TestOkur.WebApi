namespace TestOkur.WebApi.Application.Student
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Configuration;

    public sealed class GetUserStudentsQueryHandler
        : QueryHandlerAsync<GetUserStudentsQuery, IReadOnlyCollection<StudentReadModel>>
    {
        private const string Sql = @"SELECT s.*,cr.name_value, cr.grade_value,s.classroom_id
								FROM students s
								INNER JOIN classrooms cr on cr.Id=s.classroom_id	
								WHERE s.created_by=@userId
								ORDER BY s.student_number_value
                                ";

        private readonly string _connectionString;
        private readonly IProcessor _processor;

        public GetUserStudentsQueryHandler(
            ApplicationConfiguration configurationOptions,
            IProcessor processor)
        {
            _processor = processor;
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(3)]
        public override async Task<IReadOnlyCollection<StudentReadModel>> ExecuteAsync(
            GetUserStudentsQuery query,
            CancellationToken cancellationToken = default)
        {
            List<StudentReadModel> students;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                students = (await connection.QueryAsync<StudentReadModel>(
                    Sql,
                    new { userId = query.UserId })).ToList();
            }

            var contacts = await _processor.ExecuteAsync<GetUserContactsQuery, IReadOnlyCollection<ContactReadModel>>(
                new GetUserContactsQuery(query.UserId), cancellationToken);

            foreach (var student in students)
            {
                student.Contacts = contacts.Where(c => c.StudentId == student.Id)
                    .ToList();
            }

            return students;
        }
    }
}
