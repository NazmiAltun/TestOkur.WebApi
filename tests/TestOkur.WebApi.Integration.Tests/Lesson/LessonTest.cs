namespace TestOkur.WebApi.Integration.Tests.Lesson
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Serializer;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Lesson;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Integration.Tests.Common;

    public abstract class LessonTest : Test
    {
        protected const string ApiPath = "api/v1/lessons";

        protected async Task<CreateLessonCommand> CreateLessonAsync(HttpClient client)
        {
            var command = new CreateLessonCommand(
                Guid.NewGuid(),
                Random.RandomString(10));

            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            return command;
        }

        protected async Task<IEnumerable<LessonReadModel>> GetLessonListAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiPath);
            return await response.ReadAsync<IEnumerable<LessonReadModel>>();
        }
    }
}
