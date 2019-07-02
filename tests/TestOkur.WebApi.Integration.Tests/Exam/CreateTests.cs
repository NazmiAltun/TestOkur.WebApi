namespace TestOkur.WebApi.Integration.Tests.Exam
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using FluentAssertions;
	using TestOkur.Common;
	using TestOkur.Contracts.Exam;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Exam.Commands;
	using TestOkur.WebApi.Integration.Tests.Common;
	using Xunit;

	public class CreateTests : ExamTest
	{
		[Fact]
		public async Task ShouldReturnBadRequestIfExists()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateExamAsync(client);
				var response = await client.PostAsync(ApiPath, command.ToJsonContent());
				response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
				(await response.Content.ReadAsStringAsync()).Should()
					.Contain(ErrorCodes.ExamExists);
			}
		}

		[Fact]
		public async Task TrialExamShouldBeCreated()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = new CreateExamCommand(
					Guid.NewGuid(),
					"3.Trial",
					DateTime.Now,
					7,
					0,
					"Frm3rdGradeTrial",
					1,
					0,
					1,
					null,
					Random.RandomString(100));
				var response = await client.PostAsync(ApiPath, command.ToJsonContent());
				response.EnsureSuccessStatusCode();
				var exams = await GetExamListAsync(client);
				exams.Should().Contain(e => e.Name == command.Name &&
				                            e.IncorrectEliminationRate == command.IncorrectEliminationRate &&
				                            e.Notes == command.Notes &&
				                            e.ExamBookletTypeId == command.ExamBookletTypeId &&
				                            e.ExamTypeId == command.ExamTypeId);
			}
		}

		[Fact]
		public async Task ExamShouldBeCreated()
		{
			using (var testServer = await CreateWithUserAsync())
			{
				var client = testServer.CreateClient();
				var command = await CreateExamAsync(client);
				var exams = await GetExamListAsync(client);
				exams.Should().Contain(e => e.Name == command.Name &&
											e.ExamDate == command.ExamDate &&
											e.IncorrectEliminationRate == command.IncorrectEliminationRate &&
											e.Notes == command.Notes &&
											!string.IsNullOrEmpty(e.ExamTypeName) &&
											!string.IsNullOrEmpty(e.LessonName) &&
											e.ExamBookletTypeId == command.ExamBookletTypeId &&
											e.ExamTypeId == command.ExamTypeId);

				var exam = exams.First(e => e.Name == command.Name);
				var events = Consumer.Instance.GetAll<IExamCreated>();
				events.Should().Contain(e => e.ExamId == exam.Id);
			}
		}
	}
}
