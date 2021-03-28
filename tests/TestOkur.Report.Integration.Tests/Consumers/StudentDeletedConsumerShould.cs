using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;

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

    public class StudentDeletedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public StudentDeletedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task DeleteStudentForms(IFixture fixture, int userId, int examId, int studentId)
        {
            var forms = GenerateStudentForms(fixture, examId, userId, studentId);
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should()
                .HaveCount(1)
                .And
                .Contain(s => s.StudentId == studentId);
            var repository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var consumer = new StudentDeletedConsumer(repository, null);
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
