namespace TestOkur.WebApi.Application.OpticalForm
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.AspNetCore.Hosting;
    using Npgsql;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Configuration;

    public sealed class GetAllOpticalFormTypesQueryHandler : QueryHandlerAsync<GetAllOpticalFormTypesQuery, IReadOnlyCollection<OpticalFormTypeReadModel>>
    {
        private const string FormTypesSelectSql = @"SELECT oft.id,
								oft.name_value as name,
								oft.code,
								oft.configuration_file,
								oft.max_question_count,
								oft.school_type_id as school_type,
								COALESCE(fls.name_tag,L.name_value) AS lesson,
								L.id As lesson_id,
								fls.max_question_count,
								fls.form_part,
								fls.list_order
								FROM optical_form_types oft
								LEFT JOIN form_lesson_sections fls ON fls.optical_form_type_id=oft.id
								LEFT JOIN lessons L ON L.id=fls.lesson_id
								ORDER BY code,form_part,fls.list_order";

        private const string FormDefinitionSelectSql = @"
                                SELECT opt.*,name_x, name_y, surname_x, surname_y, class_x,
                                class_y, student_no_x, student_no_y, exam_name_x, exam_name_y,
                                student_no_filling_part_x, student_no_filling_part_y, optical_form_definition_id,
                                course_name_x, course_name_y, title1_x, title1_y, title2_x, title2_y
                                FROM optical_form_definitions opt
								INNER JOIN optical_form_types oft ON oft.id=opt.optical_form_type_id
                                INNER JOIN optical_form_text_locations tl on tl.optical_form_definition_id = opt.Id
								ORDER BY opt.list_order
                               ";

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _connectionString;

        public GetAllOpticalFormTypesQueryHandler(ApplicationConfiguration configurationOptions, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(1)]
        [ResultCaching(2)]
        public override async Task<IReadOnlyCollection<OpticalFormTypeReadModel>> ExecuteAsync(
            GetAllOpticalFormTypesQuery query, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var definitions = await GetDefinitionsAsync(connection);
                var formTypes = await GetFormTypesAsync(connection);
                return PopulateFormTypes(formTypes, definitions);
            }
        }

        private IReadOnlyCollection<OpticalFormTypeReadModel> PopulateFormTypes(List<OpticalFormTypeReadModel> formTypes, List<OpticalFormDefinitionReadModel> definitions)
        {
            foreach (var formType in formTypes)
            {
                var path = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    "yap",
                    formType.ConfigurationFile);
                formType.Configuration = File.ReadAllText(path);
                formType.OpticalFormDefinitions =
                    definitions.Where(d => d.OpticalFormTypeId == formType.Id)
                        .ToList();
            }

            return formTypes
                .OrderBy(f => definitions.IndexOf(f.OpticalFormDefinitions.First()))
                .ToList();
        }

        private async Task<List<OpticalFormTypeReadModel>> GetFormTypesAsync(NpgsqlConnection connection)
        {
            var dictionary = new Dictionary<int, OpticalFormTypeReadModel>();
            var formTypes = (await connection.QueryAsync<OpticalFormTypeReadModel, FormLessonSectionReadModel, OpticalFormTypeReadModel>(
                    FormTypesSelectSql,
                    (type, section) =>
                    {
                        if (!dictionary.TryGetValue(type.Id, out var formTypeEntry))
                        {
                            formTypeEntry = type;
                            dictionary.Add(formTypeEntry.Id, formTypeEntry);
                        }

                        if (section != null)
                        {
                            formTypeEntry.FormLessonSections.Add(section);
                        }

                        return formTypeEntry;
                    },
                    splitOn: "lesson"))
                .Distinct()
                .ToList();

            return formTypes;
        }

        private async Task<List<OpticalFormDefinitionReadModel>> GetDefinitionsAsync(NpgsqlConnection connection)
        {
            var formDictionary = new Dictionary<int, OpticalFormDefinitionReadModel>();

            return (await connection
                    .QueryAsync<OpticalFormDefinitionReadModel, dynamic, OpticalFormDefinitionReadModel>(
                        FormDefinitionSelectSql,
                        (form, locations) =>
                        {
                            if (!formDictionary.TryGetValue(form.Id, out var formEntry))
                            {
                                formEntry = form;
                                formDictionary.Add(formEntry.Id, formEntry);
                            }

                            formEntry.TextLocations.Add(new OpticalFormTextLocationReadModel
                            {
                                Name = new Location(locations.name_x, locations.name_y),
                                Class = new Location(locations.class_x, locations.class_y),
                                ExamName = new Location(locations.exam_name_x, locations.exam_name_y),
                                StudentNo = new Location(locations.student_no_x, locations.student_no_y),
                                StudentNoFillingPart = new Location(locations.student_no_filling_part_x, locations.student_no_filling_part_y),
                                CourseName = new Location(locations.course_name_x, locations.course_name_y),
                                Title1 = new Location(locations.title1_x, locations.title1_y),
                                Title2 = new Location(locations.title2_x, locations.title2_y),
                                Surname = new Location(locations.surname_x, locations.surname_y),
                            });

                            return formEntry;
                        },
                        splitOn: "name_x"))
                .Distinct()
                .ToList();
        }
    }
}
