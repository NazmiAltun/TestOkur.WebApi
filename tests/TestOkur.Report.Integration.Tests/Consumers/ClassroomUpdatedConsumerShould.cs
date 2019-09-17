namespace TestOkur.Report.Integration.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MassTransit;
    using NSubstitute;
    using TestOkur.Contracts.Classroom;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Repositories;
    using TestOkur.TestHelper;
    using Xunit;

    public class ClassroomUpdatedConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task UpdateClassroomName()
        {
            var classroomId = RandomGen.Next();
            var classroom = RandomGen.String(4);
            var newGrade = RandomGen.Next(11);
            var newClassName = RandomGen.String(3);
            var examId = RandomGen.Next();
            var userId = RandomGen.Next();

            using (var testServer = Create(userId))
            {
                var forms = GenerateStudentForms(examId, userId, classroomId, classroom);
                var client = testServer.CreateClient();
                var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
                response.EnsureSuccessStatusCode();
                var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
                studentOpticalForms.Should()
                    .HaveCount(2)
                    .And
                    .NotContain(s => s.ClassroomId != classroomId);
                var repository = testServer.Host.Services.GetService(typeof(IOpticalFormRepository));
                var consumer = new ClassroomUpdatedConsumer(repository as IOpticalFormRepository);
                var context = Substitute.For<ConsumeContext<IClassroomUpdated>>();
                context.Message.ClassroomId.Returns(classroomId);
                context.Message.Grade.Returns(newGrade);
                context.Message.Name.Returns(newClassName);
                await consumer.Consume(context);
                studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
                studentOpticalForms.Select(s => s.Classroom)
                    .Distinct()
                    .ToList()
                    .Should()
                    .HaveCount(1)
                    .And
                    .Contain($"{newGrade}/{newClassName}");
            }
        }

        private List<StudentOpticalForm> GenerateStudentForms(
            int examId,
            int userId,
            int classroomId,
            string classroom)
        {
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(examId, userId),
                GenerateStudentForm(examId, userId),
            };

            foreach (var form in forms)
            {
                form.ClassroomId = classroomId;
                form.Classroom = classroom;
            }

            return forms;
        }
    }
}
