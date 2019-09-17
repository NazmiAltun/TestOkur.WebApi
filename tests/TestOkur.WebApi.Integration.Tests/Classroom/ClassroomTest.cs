namespace TestOkur.WebApi.Integration.Tests.Classroom
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Domain.Model;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Classroom;
    using TestOkur.WebApi.Integration.Tests.Common;

    public abstract class ClassroomTest : Test
    {
        protected const string ApiPath = "api/v1/classrooms";

        protected async Task<CreateClassroomCommand> CreateClassroomAsync(HttpClient client)
        {
            var command = new CreateClassroomCommand(
                Guid.NewGuid(),
                Random.Next(Grade.Min, Grade.Max),
                Random.RandomString(3));

            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            return command;
        }

        protected async Task<IEnumerable<ClassroomReadModel>> GetListAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiPath);
            return await response.ReadAsync<IEnumerable<ClassroomReadModel>>();
        }
    }
}
