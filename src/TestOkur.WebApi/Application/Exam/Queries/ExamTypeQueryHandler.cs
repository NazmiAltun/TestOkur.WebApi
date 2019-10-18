namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.OpticalForm;
    using TestOkur.WebApi.Configuration;

    public sealed class ExamTypeQueryHandler : QueryHandlerAsync<ExamTypeQuery, IReadOnlyCollection<ExamTypeReadModel>>
    {
        private const string ExamTypesSelectSql = @"
                                SELECT 
                                ET.id,
                                ET.name_value AS exam_type_name,
                                ET.default_incorrect_elimination_rate_value,
                                ET.available_for_primary_school,
                                ET.available_for_high_school,
								OFT.optical_form_type_id
                                FROM exam_types ET
								INNER JOIN exam_type_optical_form_types OFT ON ET.id=OFT.exam_type_id
                                ORDER BY ET.order";

        private readonly string _connectionString;
        private readonly IQueryProcessor _queryProcessor;

        public ExamTypeQueryHandler(ApplicationConfiguration configurationOptions, IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
            _connectionString = configurationOptions.Postgres;
        }

        [ResultCaching(1)]
        public override async Task<IReadOnlyCollection<ExamTypeReadModel>> ExecuteAsync(ExamTypeQuery query, CancellationToken cancellationToken = default)
        {
            var formTypes = await GetOpticalFormTypesAsync(cancellationToken);
            var dictionary = new Dictionary<int, ExamTypeReadModel>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var examTypes = (await connection.QueryAsync<ExamTypeReadModel, dynamic, ExamTypeReadModel>(
                    ExamTypesSelectSql,
                    (type, _) =>
                    {
                        if (!dictionary.TryGetValue(type.Id, out var examTypeEntry))
                        {
                            examTypeEntry = type;
                            dictionary.Add(examTypeEntry.Id, examTypeEntry);
                        }

                        if (examTypeEntry.OpticalFormTypes.All(x => x.Id != _.optical_form_type_id))
                        {
                            examTypeEntry.OpticalFormTypes.Add(
                                formTypes.First(f => f.Id == _.optical_form_type_id));
                        }

                        return examTypeEntry;
                    },
                    splitOn: "optical_form_type_id"))
                    .Distinct()
                    .ToList();

                return examTypes;
            }
        }

        private async Task<IReadOnlyCollection<OpticalFormTypeReadModel>> GetOpticalFormTypesAsync(CancellationToken cancellationToken)
        {
            return await _queryProcessor
                .ExecuteAsync(new GetAllOpticalFormTypesQuery(), cancellationToken);
        }
    }
}
