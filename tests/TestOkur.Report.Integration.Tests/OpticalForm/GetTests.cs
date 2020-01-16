using TestOkur.Serialization;

namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Integration.Tests.Consumers;
    using TestOkur.TestHelper;
    using Xunit;

    public class GetTests : ConsumerTest
    {
        [Fact]
        public async Task GiveStudentEndpoint_Should_ReturnAllOpticalFormsOfStudent()
        {
            var userId = RandomGen.Next();
            var studentId = RandomGen.Next();

            using var testServer = Create(userId);
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(RandomGen.Next(), userId),
                GenerateStudentForm(RandomGen.Next(), userId),
                GenerateStudentForm(RandomGen.Next(), userId),
                GenerateStudentForm(RandomGen.Next(), userId),
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

        [Fact]
        public async Task ShouldReturnAnswerKeyForms()
        {
            using var testServer = Create();
            var examId = await ExecuteExamCreatedConsumerAsync(testServer);
            var client = testServer.CreateClient();
            var examForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            examForms.SelectMany(e => e.Sections)
                .Should()
                .NotContain(s => string.IsNullOrEmpty(s.LessonName) ||
                                 s.LessonId == default);
        }
    }
}
