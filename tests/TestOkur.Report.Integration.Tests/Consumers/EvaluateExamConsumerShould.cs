using Microsoft.Extensions.DependencyInjection;
using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.Consumers
{
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Consumers;
    using TestOkur.Report.Domain;
    using TestOkur.Report.Domain.Statistics;
    using TestOkur.Report.Infrastructure.Repositories;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using TestOkur.Test.Common.Extensions;
    using Xunit;

    public class EvaluateExamConsumerShould : ConsumerTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public EvaluateExamConsumerShould(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldEvaluateAndSaveResults(IFixture fixture, int userId, int examId)
        {
            var answerKeyForms = GenerateAnswerKeyOpticalForms(fixture, 1).ToList();

            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var repo = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>();
            await ExecuteExamCreatedConsumerAsync(repo, answerKeyForms, examId);
            var studentOpticalFormRepository =
                _webApplicationFactory.Services.GetRequiredService<IStudentOpticalFormRepository>();
            var examStatisticsRepository = _webApplicationFactory.Services.GetRequiredService<IExamStatisticsRepository>(); 
            var answerKeyOpticalFormRepository = _webApplicationFactory.Services.GetRequiredService<IAnswerKeyOpticalFormRepository>(); 
            var logger = _webApplicationFactory.Services.GetRequiredService<ILogger<EvaluateExamConsumer>>();

            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
            };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();

            var consumer = new EvaluateExamConsumer(
                studentOpticalFormRepository,
                logger,
                new Evaluator(),
                answerKeyOpticalFormRepository,
                null,
                examStatisticsRepository);
            await consumer.ConsumeAsync(examId);
            var studentOpticalForms = await GetListAsync<StudentOpticalForm>(client, examId);
            studentOpticalForms.Should().HaveCount(forms.Count);
            studentOpticalForms.Should().NotContain(s => !s.Orders.Any());
            response = await client.GetAsync($"api/v1/exam-statistics/{examId}");
            var examStats = await response.ReadAsync<ExamStatistics>();
            examStats.GeneralAttendanceCount.Should().Be(forms.Count);
            examStats.CityAttendanceCounts.First().Value.Should().Be(forms.Count);
            examStats.ClassroomAttendanceCounts.First().Value.Should().Be(forms.Count);
            examStats.DistrictAttendanceCounts.First().Value.Should().Be(forms.Count);
            examStats.SchoolAttendanceCounts.First().Value.Should().Be(forms.Count);
        }
    }
}
