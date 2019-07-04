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
	using TestOkur.Infrastructure.Cqrs;
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
                                SELECT opt.*, 
                                tl.name_x as X,tl.name_y as Y,
                                tl.surname_x as X, tl.surname_y as Y,
                                tl.class_x as X, tl.class_y as Y,
                                tl.student_no_x as X, tl.student_no_y as Y,
                                tl.exam_name_x as X, tl.exam_name_y as Y,
                                tl.student_no_filling_part_x as X, tl.student_no_filling_part_y as Y
                                FROM optical_form_definitions opt
								INNER JOIN optical_form_types oft ON oft.id=opt.optical_form_type_id
                                INNER JOIN optical_form_text_locations tl on tl.optical_form_definition_id = opt.Id
								ORDER BY opt.description
                               ";

		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly string _connectionString;

		public GetAllOpticalFormTypesQueryHandler(ApplicationConfiguration configurationOptions, IHostingEnvironment hostingEnvironment)
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

			return formTypes;
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
					.QueryAsync<OpticalFormDefinitionReadModel, Location, Location, Location, Location, Location, Location,
						OpticalFormDefinitionReadModel>(
						FormDefinitionSelectSql,
						(form, nameLoc, surnameLoc, classLoc, studentNoLoc, examNameLoc, studentNoFillingPartLoc) =>
						{
							if (!formDictionary.TryGetValue(form.Id, out var formEntry))
							{
								formEntry = form;
								formDictionary.Add(formEntry.Id, formEntry);
							}

							formEntry.TextLocations.Add(new OpticalFormTextLocationReadModel
							{
								Name = nameLoc,
								Class = classLoc,
								ExamName = examNameLoc,
								StudentNo = studentNoLoc,
								StudentNoFillingPart = studentNoFillingPartLoc,
								Surname = surnameLoc,
							});

							return formEntry;
						},
						splitOn: "X,X,X,X,X,X"))
				.Distinct()
				.ToList();
		}
	}
}
