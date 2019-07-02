namespace TestOkur.WebApi.Integration.Tests.Exam
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using TestOkur.TestHelper.Extensions;
	using TestOkur.WebApi.Application.Exam.Commands;
	using TestOkur.WebApi.Application.Exam.Queries;
	using TestOkur.WebApi.Integration.Tests.Common;

	public abstract class ExamTest : Test
	{
		protected const string ApiPath = "api/v1/exams";

		protected async Task<CreateExamCommand> CreateExamAsync(
			HttpClient client)
		{
			var examType = await GetRandomExamTypeAsync(client);

			var command = new CreateExamCommand(
				Guid.NewGuid(),
				Random.RandomString(100),
				DateTime.Today,
				examType.Id,
				3,
				examType.OpticalFormTypes.Random().Code,
				1,
				1 + Random.Next(10),
				2,
				null,
				Random.RandomString(200));
			var response = await client.PostAsync(ApiPath, command.ToJsonContent());
			response.EnsureSuccessStatusCode();

			return command;
		}

		protected async Task<IEnumerable<ExamReadModel>> GetExamListAsync(HttpClient client)
		{
			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<ExamReadModel>>();
		}

		protected async Task<HttpResponseMessage> DeleteExamAsync(HttpClient client, int id)
		{
			return await client.DeleteAsync($"{ApiPath}/{id}");
		}

		protected async Task<ExamTypeReadModel> GetRandomExamTypeAsync(HttpClient client)
		{
			const string ApiPath = "api/v1/exam-types";
			var response = await client.GetAsync(ApiPath);
			var examTypes = await response.ReadAsync<IEnumerable<ExamTypeReadModel>>();

			return examTypes.Random();
		}
	}
}
