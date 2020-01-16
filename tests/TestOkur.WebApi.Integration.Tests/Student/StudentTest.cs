namespace TestOkur.WebApi.Integration.Tests.Student
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Domain.Model;
    using TestOkur.Serialization;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Classroom;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Application.Student;
    using TestOkur.WebApi.Integration.Tests.Common;

    public abstract class StudentTest : Test
    {
        protected const string ApiPath = "api/v1/students";
        protected const string Numbers = "123456789";

        protected async Task<IEnumerable<StudentReadModel>> GetListAsync(HttpClient client)
        {
            var response = await client.GetAsync(ApiPath);
            return await response.ReadAsync<IEnumerable<StudentReadModel>>();
        }

        protected async Task<IEnumerable<CreateStudentCommand>> GenerateCommandsAsync(
            HttpClient client, int count)
        {
            var classroomId = await GetClassroomIdAsync(client);
            var list = new List<CreateStudentCommand>();

            for (var i = 0; i < count; i++)
            {
                list.Add(GenerateCommand(classroomId));
            }

            return list;
        }

        protected async Task<CreateStudentCommand> CreateStudentAsync(HttpClient client)
        {
            var classroomId = await GetClassroomIdAsync(client);
            var command = GenerateCommand(classroomId);
            await client.PostAsync(ApiPath, command.ToJsonContent());

            return command;
        }

        protected async Task<int> GetClassroomIdAsync(HttpClient client)
        {
            const string ApiPath = "api/v1/classrooms";

            var command = new CreateClassroomCommand(
                    Guid.NewGuid(),
                    Random.Next(Grade.Min, Grade.Max),
                    Random.RandomString(3));
            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            response = await client.GetAsync(ApiPath);
            var list = await response.ReadAsync<IEnumerable<ClassroomReadModel>>();

            return list.First().Id;
        }

        private CreateStudentCommand GenerateCommand(int classroomId)
        {
            var contacts = new List<CreateContactCommand>
            {
                new CreateContactCommand(
                    Guid.NewGuid(),
                    Random.RandomString(10),
                    Random.RandomString(10),
                    RandomGen.Phone(),
                    1),
                new CreateContactCommand(
                    Guid.NewGuid(),
                    Random.RandomString(10),
                    Random.RandomString(10),
                    RandomGen.Phone(),
                    1),
            };

            return new CreateStudentCommand(
                Guid.NewGuid(),
                Random.RandomString(6),
                Random.RandomString(8),
                Random.Next(StudentNumber.Min, StudentNumber.Max),
                classroomId,
                Random.RandomString(200),
                "Single",
                null,
                contacts);
        }
    }
}
