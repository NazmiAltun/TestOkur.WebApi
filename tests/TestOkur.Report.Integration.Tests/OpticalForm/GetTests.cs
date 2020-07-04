using TestOkur.Serialization;

namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using AutoFixture;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Integration.Tests.Consumers;
    using TestOkur.Test.Common;
    using Xunit;

    public class GetTests : ConsumerTest
    {
        [Theory]
        [TestOkurAutoData]
        public async Task GiveStudentEndpoint_Should_ReturnAllOpticalFormsOfStudent(IFixture fixture, int userId, int studentId)
        {
            using var testServer = Create(userId);
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,fixture.Create<int>(), userId),
                GenerateStudentForm(fixture,fixture.Create<int>(), userId),
                GenerateStudentForm(fixture,fixture.Create<int>(), userId),
                GenerateStudentForm(fixture,fixture.Create<int>(), userId),
            };
            foreach (var form in forms)
            {
                form.StudentId = studentId;
            }

            var client = testServer.CreateClient();
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var returnedForms = await GetStudentFormsByStudentIdAsync(client, studentId);
            returnedForms.Should().HaveCount(forms.Count);
        }

        [Theory]
        [TestOkurAutoData]
        public async Task ShouldReturnAnswerKeyForms(IFixture fixture)
        {
            using var testServer = Create(fixture.Create<int>());
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, fixture);
            var client = testServer.CreateClient();
            var examForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            examForms.SelectMany(e => e.Sections)
                .Should()
                .NotContain(s => string.IsNullOrEmpty(s.LessonName) ||
                                 s.LessonId == default);
        }
    }
}
