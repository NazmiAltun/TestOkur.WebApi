namespace TestOkur.WebApi.Integration.Tests.Student
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Application.Student;
    using Xunit;

    public class CreateTests : StudentTest
    {
        [Fact]
        public async Task Should_Create_In_Bulk_When_Phone_Is_Missing()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var classroomId = await GetClassroomIdAsync(client);
#pragma warning disable SA1118 // ParameterMustNotSpanMultipleLines
            var command = new BulkCreateStudentCommand(
                Guid.NewGuid(),
                new[]
                {
                    new CreateStudentCommand(
                        Guid.NewGuid(),
                        "A",
                        "B",
                        RandomGen.Next(1000),
                        classroomId,
                        null,
                        "Single",
                        new[]
                        {
                            new CreateContactCommand(
                                Guid.NewGuid(),
                                "A",
                                "B",
                                string.Empty,
                                2),
                        }),
                });
#pragma warning restore SA1118 // ParameterMustNotSpanMultipleLines
            var response = await client.PostAsync($"{ApiPath}/bulk", command.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var list = await GetListAsync(client);

            list.Should().HaveCountGreaterOrEqualTo(command.Commands.Count());
        }

        [Fact]
        public async Task ShouldCreateStudentsInBulk()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command = new BulkCreateStudentCommand(
                Guid.NewGuid(),
                await GenerateCommandsAsync(client, 15));
            var response = await client.PostAsync($"{ApiPath}/bulk", command.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var list = await GetListAsync(client);

            list.Should().HaveCountGreaterOrEqualTo(command.Commands.Count());
        }

        [Fact]
        public async Task When_ValidModelPosted_Then_StudentRecordShouldBeCreated()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command = await CreateStudentAsync(client);
            var list = await GetListAsync(client);
            list.Should().Contain(s => s.StudentNumber == command.StudentNumber &&
                                       s.FirstName == command.FirstName &&
                                       s.StudentNumber == command.StudentNumber &&
                                       s.ClassroomId == command.ClassroomId &&
                                       s.Contacts.Any(c => c.Phone == command.Contacts.First().Phone) &&
                                       s.Contacts.Any(c => c.Phone == command.Contacts.Last().Phone));
        }

        [Theory]
        [InlineData("", "Black", 123, 4, ErrorCodes.FirstNameCannotBeEmpty)]
        [InlineData("Jack", "", 123, 4, ErrorCodes.LastNameCannotBeEmpty)]
        [InlineData("Jack", "Black", 0, 4, ErrorCodes.InvalidStudentNo)]
        [InlineData("Jack", "Black", 123, 0, ErrorCodes.ClassroomDoesNotExist)]
        public async Task When_InvalidModelPosted_Then_Server_Should_Return_BadRequest(
            string firstName,
            string lastName,
            int studentNumber,
            int classroomId,
            string errorMessage)
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var command = new CreateStudentCommand(
                Guid.NewGuid(),
                firstName,
                lastName,
                studentNumber,
                classroomId,
                Random.RandomString(200),
                "Single",
                null);

            var response = await client.PostAsync(ApiPath, command.ToJsonContent());
            await response.Should().BeBadRequestAsync(errorMessage);
        }
    }
}
