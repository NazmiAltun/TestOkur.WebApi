namespace TestOkur.WebApi.Integration.Tests.User
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Domain.Model;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Classroom;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Application.Lesson.Commands;
    using TestOkur.WebApi.Application.Student;
    using TestOkur.WebApi.Application.User.Queries;
    using Xunit;

    public class GetUserTests : UserTest
    {
        private const string Numbers = "123456789";

        [Fact]
        public async Task GivenGetRecords_ShouldReturnCountOfUserRecords()
        {
            using (var testServer = await CreateWithUserAsync())
            {
                var client = testServer.CreateClient();
                for (var i = 0; i < 2; i++)
                {
                    await CreateClassroomAsync(client);
                    await CreateLessonAsync(client);
                    await CreateStudentAsync(client);
                }

                var response = await client.GetAsync($"{ApiPath}/record-counts");
                response.EnsureSuccessStatusCode();
                var records = await response.ReadAsync<UserRecords>();
                records.ClassroomCount.Should().Be(2);
                records.LessonCount.Should().Be(2);
                records.StudentCount.Should().Be(2);
                records.ExamCount.Should().Be(0);
            }
        }

        [Fact]
        public async Task GivenGetUsers_ShouldReturnSeededUsers()
        {
            var client = (await GetTestServer()).CreateClient();
            var response = await client.GetAsync(ApiPath);
            var users = await response.ReadAsync<IReadOnlyCollection<UserReadModel>>();
            users.Should().NotBeEmpty();
        }

        private async Task CreateClassroomAsync(HttpClient client)
        {
            const string ApiPath = "api/v1/classrooms";
            var command = new CreateClassroomCommand(
                    Guid.NewGuid(),
                    Random.Next(Grade.Min, Grade.Max),
                    Random.RandomString(3));

            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();
        }

        private async Task CreateStudentAsync(HttpClient client)
        {
            const string ApiPath = "api/v1/students";
            const string ClassroomApiPath = "api/v1/classrooms";

            var response = await client.GetAsync(ClassroomApiPath);
            var list = await response.ReadAsync<IEnumerable<ClassroomReadModel>>();
            var command = GenerateCommand(list.Random().Id);
            await client.PostAsync(ApiPath, command.ToJsonContent());
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
                    2),
            };

            return new CreateStudentCommand(
                Guid.NewGuid(),
                Random.RandomString(6),
                Random.RandomString(8),
                Random.Next(StudentNumber.Min, StudentNumber.Max),
                classroomId,
                Random.RandomString(200),
                "Single",
                contacts);
        }

        private async Task<CreateLessonCommand> CreateLessonAsync(HttpClient client)
        {
            const string ApiPath = "api/v1/lessons";

            var command = new CreateLessonCommand(
                    Guid.NewGuid(),
                    Random.RandomString(10));

            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            response.EnsureSuccessStatusCode();

            return command;
        }
    }
}
