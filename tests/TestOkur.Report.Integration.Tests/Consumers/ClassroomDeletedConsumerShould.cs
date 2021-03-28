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
    using TestOkur.Contracts.Classroom;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class ClassroomDeletedConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public ClassroomDeletedConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task DeleteStudentFormsOfClassrooms(IFixture fixture, int userId, int examId, int classroomId)
        {
            var forms = GenerateStudentForms(fixture, examId, userId, classroomId);
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should()
                .HaveCount(2)
                .And
                .NotContain(s => s.ClassroomId != classroomId);
            var repository = _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var consumer = new ClassroomDeletedConsumer(repository, null);
            var context = Substitute.For<ConsumeContext<IClassroomDeleted>>();
            context.Message.ClassroomId.Returns(classroomId);
            await consumer.Consume(context);
            studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should().BeEmpty();
        }

        private List<StudentOpticalForm> GenerateStudentForms(IFixture fixture, int examId, int userId, int classroomId)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
            };

            foreach (var form in forms)
            {
                form.ClassroomId = classroomId;
            }

            return forms;
        }
    }
}
