using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;
using TestOkur.Serialization;

namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using AutoFixture;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Report.Integration.Tests.Consumers;
    using TestOkur.Test.Common;
    using Xunit;

    public class GetTests : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public GetTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }
        
        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task GiveStudentEndpoint_Should_ReturnAllOpticalFormsOfStudent(IFixture fixture, int userId, int studentId)
        {
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

            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var returnedForms = await GetStudentFormsByStudentIdAsync(client, studentId);
            returnedForms.Should().HaveCount(forms.Count);
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldReturnAnswerKeyForms(IFixture fixture)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(fixture.Create<int>());
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            var examId = await ExecuteExamCreatedConsumerAsync(repo, fixture);
            var examForms = await GetListAsync<AnswerKeyOpticalForm>(client, examId);
            examForms.SelectMany(e => e.Sections)
                .Should()
                .NotContain(s => string.IsNullOrEmpty(s.LessonName) ||
                                 s.LessonId == default);
        }
    }
}
