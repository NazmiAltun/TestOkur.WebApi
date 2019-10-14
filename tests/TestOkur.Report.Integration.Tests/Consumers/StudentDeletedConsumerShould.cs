namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
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

    public class StudentDeletedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task DeleteStudentForms()
        {
            var examId = RandomGen.Next();
            var userId = RandomGen.Next();
            var studentId = RandomGen.Next();

            using (var testServer = Create(userId))
            {
                var forms = GenerateStudentForms(examId, userId, studentId);
                var client = testServer.CreateClient();
                var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
                response.EnsureSuccessStatusCode();
                var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
                studentOpticalForms.Should()
                    .HaveCount(1)
                    .And
                    .Contain(s => s.StudentId == studentId);
                var repository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository));
                var consumer = new StudentDeletedConsumer(repository as IStudentOpticalFormRepository, null);
                var context = Substitute.For<ConsumeContext<IStudentDeleted>>();
                context.Message.StudentId.Returns(studentId);
                await consumer.Consume(context);
                studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
                studentOpticalForms.Should().BeEmpty();
            }
        }

        private List<StudentOpticalForm> GenerateStudentForms(int examId, int userId, int studentId)
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
