namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.TestHelper;
    using Xunit;
    using TestOkur.Serializer;

    public class ClassroomDeletedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task DeleteStudentFormsOfClassrooms()
        {
            var classroomId = RandomGen.Next();
            var examId = RandomGen.Next();
            var userId = RandomGen.Next();

            using var testServer = Create(userId);
            var forms = GenerateStudentForms(examId, userId, classroomId);
            var client = testServer.CreateClient();
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should()
                .HaveCount(2)
                .And
                .NotContain(s => s.ClassroomId != classroomId);
            var repository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository));
            var consumer = new ClassroomDeletedConsumer(repository as IStudentOpticalFormRepository, null);
            var context = Substitute.For<ConsumeContext<IClassroomDeleted>>();
            context.Message.ClassroomId.Returns(classroomId);
            await consumer.Consume(context);
            studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should().BeEmpty();
        }

        private List<StudentOpticalForm> GenerateStudentForms(int examId, int userId, int classroomId)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(examId, userId),
                GenerateStudentForm(examId, userId),
            };

            foreach (var form in forms)
            {
                form.ClassroomId = classroomId;
            }

            return forms;
        }
    }
}
