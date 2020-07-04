namespace TestOkur.Report.Integration.Tests.Consumers
{
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class StudentDeletedConsumerShould : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task DeleteStudentForms(IFixture fixture, int userId, int examId, int studentId)
        {
            using var testServer = Create(userId);
            var forms = GenerateStudentForms(fixture, examId, userId, studentId);
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

        private List<StudentOpticalForm> GenerateStudentForms(IFixture fixture, int examId, int userId, int studentId)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture, examId, userId),
            };

            foreach (var form in forms)
            {
                form.StudentId = studentId;
            }

            return forms;
        }
    }
}
