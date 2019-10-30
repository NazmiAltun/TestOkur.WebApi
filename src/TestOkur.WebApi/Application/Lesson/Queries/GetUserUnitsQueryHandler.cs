namespace TestOkur.WebApi.Application.Lesson.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Configuration;

    public sealed class GetUserUnitsQueryHandler
        : QueryHandlerAsync<GetUserUnitsQuery, IReadOnlyCollection<UnitReadModel>>
    {
        private const string Sql = @"SELECT u.Id,
								u.name_value as name,
								u.lesson_id,
								l.name_value as lesson,
								u.grade_value as grade,
                                u.shared,
								s.id as subject_id,
								s.name_value as subject_name,
                                s.shared as shared_subject
								FROM units u
								INNER JOIN lessons l on l.id=u.lesson_id
								LEFT JOIN subjects s on s.unit_id=u.id
								WHERE u.created_by=@userId
								ORDER BY u.name_value,s.name_value";

        private readonly string _connectionString;

        public GetUserUnitsQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(3)]
        public override async Task<IReadOnlyCollection<UnitReadModel>> ExecuteAsync(
            GetUserUnitsQuery query,
            CancellationToken cancellationToken = default)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            var dictionary = new Dictionary<int, UnitReadModel>();

            return (await connection.QueryAsync<UnitReadModel, SubjectReadModel, UnitReadModel>(
                    Sql,
                    (unit, subject) =>
                    {
                        if (!dictionary.TryGetValue(unit.Id, out var unitEntry))
                        {
                            unitEntry = unit;
                            dictionary.Add(unitEntry.Id, unitEntry);
                        }

                        if (subject != null)
                        {
                            unitEntry.Subjects.Add(subject);
                        }

                        return unitEntry;
                    },
                    new { query.UserId },
                    splitOn: "subject_id"))
                .Distinct()
                .ToList();
        }
    }
}
