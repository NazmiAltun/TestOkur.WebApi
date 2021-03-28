using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.Consumers
{
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using TestOkur.Contracts.Student;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class StudentUpdatedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public StudentUpdatedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task UpdateStudentInfoInStudentForms(IFixture fixture, int userId, int examId,
            int studentId, int classroomId, string firstName, string lastName, int studentNumber)
        {
            var forms = GenerateStudentForms(fixture, examId, userId, studentId);
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should()
                .HaveCount(1);
            var repository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var consumer = new StudentUpdatedConsumer(repository);
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
            IFixture fixture,
            int examId,
            int userId,
            int studentId)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,examId, userId),
            };

            foreach (var form in forms)
            {
                form.StudentId = studentId;
            }

            return forms;
        }
    }
}
