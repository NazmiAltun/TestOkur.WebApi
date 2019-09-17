namespace TestOkur.WebApi.Integration.Tests.Lesson.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Domain.Model;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;

    public abstract class UnitTest : Test
	{
		protected const string ApiPath = "api/v1/units";

		protected async Task<AddSubjectCommand> AddSubjectAsync(HttpClient client)
		{
			await CreateUnitAsync(client);
			var units = await GetUnitListAsync(client);
			var addSubjectCommand = new AddSubjectCommand(
					Guid.NewGuid(),
					Random.RandomString(10),
					units.Last().Id);

			var response = await client.PostAsync(
				$"{ApiPath}/{addSubjectCommand.UnitId}/subjects",
				addSubjectCommand.ToJsonContent());
			response.EnsureSuccessStatusCode();

			return addSubjectCommand;
		}

		protected async Task<CreateUnitCommand> CreateUnitAsync(HttpClient client)
		{
			var command = new CreateUnitCommand(
				Guid.NewGuid(),
				Random.RandomString(50),
				(await GetLessonListAsync(client)).Random().Id,
				Random.Next(Grade.Min, Grade.Max));

			var response = await client.PostAsync(ApiPath, command.ToJsonContent());
			response.EnsureSuccessStatusCode();

			return command;
		}

		protected async Task<IEnumerable<UnitReadModel>> GetUnitListAsync(HttpClient client)
		{
			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<UnitReadModel>>();
		}

		protected async Task<IEnumerable<LessonReadModel>> GetLessonListAsync(HttpClient client)
		{
			const string ApiPath = "api/v1/lessons/shared";
			var response = await client.GetAsync(ApiPath);
			return await response.ReadAsync<IEnumerable<LessonReadModel>>();
		}
	}
}
