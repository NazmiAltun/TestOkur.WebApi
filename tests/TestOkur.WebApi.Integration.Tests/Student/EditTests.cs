namespace TestOkur.WebApi.Integration.Tests.Student
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Contracts.Student;
    using TestOkur.Domain.Model;
    using TestOkur.Serialization;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using TestOkur.WebApi.Application.Contact;
    using TestOkur.WebApi.Application.Student;
    using TestOkur.WebApi.Integration.Tests.Common;
    using Xunit;

    public class EditTests : StudentTest
    {
       [Fact(Skip = "Fix later")]
        public async Task WhenNotExistingValuesPosted_Then_ShouldEdit()
        {
            using var testServer = await CreateWithUserAsync();
            var client = testServer.CreateClient();
            var createCommand = await CreateStudentAsync(client);
            var list = await GetListAsync(client);

            var editCommand = new EditStudentCommand(
                Guid.NewGuid(),
                list.First(s => s.StudentNumber == createCommand.StudentNumber).Id,
                Random.RandomString(5),
                Random.RandomString(5),
                Random.Next(StudentNumber.Min, StudentNumber.Max),
                createCommand.ClassroomId,
                Random.RandomString(300),
                null,
                new[]
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
                }.ToList());
            await client.PutAsync(ApiPath, editCommand.ToJsonContent());
            list = await GetListAsync(client);
            list.Should().Contain(s => s.StudentNumber == editCommand.NewStudentNumber &&
                                       s.Contacts.Count == editCommand.Contacts.Count &&
                                       s.ClassroomId == editCommand.NewClassroomId &&
                                       s.Contacts.Any(c => c.Phone == editCommand.Contacts.First().Phone) &&
                                       s.FirstName == editCommand.NewFirstName &&
                                       s.LastName == editCommand.NewLastName &&
                                       s.CitizenshipIdentity == editCommand.CitizenshipIdentity)
                .And
                .NotContain(s => s.StudentNumber == createCommand.StudentNumber);
            var @event = Consumer.Instance.GetFirst<IStudentUpdated>();
            @event.StudentId.Should().Be(editCommand.StudentId);
            @event.ClassroomId.Should().Be(editCommand.NewClassroomId);
            @event.FirstName.Should().Be(editCommand.NewFirstName);
            @event.LastName.Should().Be(editCommand.NewLastName);
            @event.StudentNumber.Should().Be(editCommand.NewStudentNumber);
        }
    }
}
