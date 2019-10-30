namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.TestHelper;
    using Xunit;

    public class StudentUpdatedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task UpdateStudentInfoInStudentForms()
        {
            var examId = RandomGen.Next();
            var userId = RandomGen.Next();
            var studentId = RandomGen.Next();
            var firstName = RandomGen.String(20);
            var lastName = RandomGen.String(20);
            var classroomId = RandomGen.Next();
            var studentNumber = RandomGen.Next(1000);

            using var testServer = Create(userId);
            var forms = GenerateStudentForms(examId, userId, studentId);
            var client = testServer.CreateClient();
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should()
                .HaveCount(1);
            var repository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository));
            var consumer = new StudentUpdatedConsumer(repository as IStudentOpticalFormRepository);
            var context = Substitute.For<ConsumeContext<IStudentUpdated>>();
            context.Message.ClassroomId.Returns(classroomId);
            context.Message.FirstName.Returns(firstName);
            context.Message.LastName.Returns(lastName);
            context.Message.StudentId.Returns(studentId);
            context.Message.StudentNumber.Returns(studentNumber);
            await consumer.Consume(context);
            studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.First()
                .Should().Match<StudentOpticalForm>(
                    x => x.StudentId == studentId &&
                         x.ClassroomId == classroomId &&
                         x.StudentFirstName == firstName &&
                         x.StudentLastName == lastName &&
                         x.StudentNumber == studentNumber);
        }

        private List<StudentOpticalForm> GenerateStudentForms(
            int examId,
            int userId,
            int studentId)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(examId, userId),
            };

            foreach (var form in forms)
            {
                form.StudentId = studentId;
            }

            return forms;
        }
    }
}
