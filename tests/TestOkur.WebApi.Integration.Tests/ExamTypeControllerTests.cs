namespace TestOkur.WebApi.Integration.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Domain.Model.ExamModel;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Exam.Queries;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class ExamTypeControllerTests : Test
	{
		private const string ApiPath = "api/v1/exam-types";

		[Fact]
		public async Task GetExamTypes_Should_Return_AllExamTypes()
		{
			using (var testServer = await CreateAsync())
			{
				var client = testServer.CreateClient();
				var response = await client.GetAsync(ApiPath);
				var examTypes = await response.ReadAsync<IEnumerable<ExamTypeReadModel>>();

				examTypes.Should().Contain(e => e.Name == ExamTypes.LessonExam)
					.And
					.Contain(e => e.Name == ExamTypes.LessonExam &&
                                  e.AvailableForHighSchool &&
                                  e.AvailableForPrimarySchool &&
								  e.OpticalFormTypes.Count() == 9);
				examTypes.Should().Contain(e => e.Name == ExamTypes.EvaluationExam)
					.And
					.Contain(e => e.Name == ExamTypes.EvaluationExam &&
                                  e.AvailableForHighSchool &&
                                  e.AvailableForPrimarySchool &&
                                  e.OpticalFormTypes.Count() == 9);
                examTypes.Should().Contain(e => e.Name == ExamTypes.Tyt &&
                                  e.AvailableForHighSchool &&
                                  !e.AvailableForPrimarySchool);

                examTypes.Should().Contain(e => e.Name == ExamTypes.Lgs &&
                                                !e.AvailableForHighSchool &&
                                                e.AvailableForPrimarySchool);

                examTypes.Should().Contain(e => e.Name == ExamTypes.Ayt &&
                                                e.AvailableForHighSchool &&
                                                !e.AvailableForPrimarySchool);

                examTypes.Should().Contain(e => e.Name == ExamTypes.AytLang &&
                                                e.AvailableForHighSchool &&
                                                !e.AvailableForPrimarySchool);

                examTypes.Should().Contain(e => e.Name == ExamTypes.Scholarship &&
                                                e.AvailableForHighSchool &&
                                                e.AvailableForPrimarySchool);

                examTypes.Should().Contain(e => e.Name == ExamTypes.TrialExam &&
                                                e.AvailableForHighSchool &&
                                                e.AvailableForPrimarySchool);

                examTypes.Should().NotContain(e => !e.OpticalFormTypes.Any());

				examTypes.Select(e => e.Name)
					.Should().ContainInOrder(
						ExamTypes.LessonExam,
						ExamTypes.EvaluationExam,
						ExamTypes.TrialExam,
						ExamTypes.Lgs,
						ExamTypes.Scholarship,
						ExamTypes.Tyt,
						ExamTypes.Ayt,
						ExamTypes.AytLang);
			}
		}
	}
}
