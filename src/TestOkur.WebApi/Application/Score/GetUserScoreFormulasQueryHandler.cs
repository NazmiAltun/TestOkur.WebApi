namespace TestOkur.WebApi.Application.Score
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Dapper;
	using Npgsql;
	using Paramore.Darker;
	using Paramore.Darker.QueryLogging;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.Infrastructure.Cqrs;
	using TestOkur.WebApi.Configuration;

	public class GetUserScoreFormulasQueryHandler
		: QueryHandlerAsync<GetUserScoreFormulasQuery, IReadOnlyCollection<ScoreFormulaReadModel>>
	{
		private const string Sql = @"SELECT 
								sf.id,
								sf.grade_value as grade,
								sf.name_value as score_name,
								sf.exam_id,
								base_point,
								formula_type_id,
								ft.name as formula_type,
								lc.id as lesson_coefficient_id,
								lc.coefficient,
								coalesce(els.name_tag,l.name_value) as lesson,
								et.name_value as exam_type,
								et.id as exam_type_id,
								els.lesson_id
								FROM score_formulas sf
								INNER JOIN formula_types ft ON sf.formula_type_id=ft.id
								INNER JOIN lesson_coefficients lc ON lc.score_formula_id=sf.id
								INNER JOIN form_lesson_sections els ON els.id=lc.exam_lesson_section_id
								INNER JOIN optical_form_types oft ON oft.id=els.optical_form_type_id
								INNER JOIN exam_type_optical_form_types etoft ON etoft.optical_form_type_id=oft.id
								INNER JOIN exam_types et ON et.id=etoft.exam_type_id
								INNER JOIN lessons l ON l.id=els.lesson_id
								WHERE sf.created_by=@userId
								ORDER BY score_name,els.list_order";

		private readonly string _connectionString;

		public GetUserScoreFormulasQueryHandler(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		[PopulateQuery(1)]
		[QueryLogging(2)]
		[ResultCaching(3)]
		public override async Task<IReadOnlyCollection<ScoreFormulaReadModel>> ExecuteAsync(
			GetUserScoreFormulasQuery query,
			CancellationToken cancellationToken = default)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				var dict = new Dictionary<int, ScoreFormulaReadModel>();

				return (await connection
						.QueryAsync<ScoreFormulaReadModel, LessonCoefficientReadModel, ScoreFormulaReadModel>(
							Sql,
							(scoreFormula, coefficient) =>
							{
								if (!dict.TryGetValue(scoreFormula.Id, out var scoreFormulaEntry))
								{
									scoreFormulaEntry = scoreFormula;
									dict.Add(scoreFormulaEntry.Id, scoreFormulaEntry);
								}

								scoreFormulaEntry.Coefficients.Add(coefficient);

								return scoreFormulaEntry;
							},
							new { query.UserId },
							splitOn: "lesson_coefficient_id"))
					.Distinct()
					.ToList();
			}
		}
	}
}
