namespace TestOkur.Report.Integration.Tests.Consumers
{
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
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;
    using Xunit;

    public class EvaluateExamConsumerShould : ConsumerTest
    {
        [Fact]
        public async Task ShouldEvaluateAndSaveResults()
        {
            var userId = RandomGen.Next(10000);
            var answerKeyForms = GenerateAnswerKeyOpticalForms(1).ToList();

            using var testServer = Create(userId);
            var client = testServer.CreateClient();
            var examId = await ExecuteExamCreatedConsumerAsync(testServer, answerKeyForms);
            var studentOpticalFormRepository = testServer.Host.Services.GetService(typeof(IStudentOpticalFormRepository))
                as IStudentOpticalFormRepository;
            var examStatisticsRepository = testServer.Host.Services.GetService(typeof(IExamStatisticsRepository))
                as IExamStatisticsRepository;
            var answerKeyOpticalFormRepository = testServer.Host.Services.GetService(typeof(IAnswerKeyOpticalFormRepository))
                as IAnswerKeyOpticalFormRepository;
            var logger = testServer.Host.Services.GetService(typeof(ILogger<EvaluateExamConsumer>));
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(examId, userId),
                GenerateStudentForm(examId, userId),
            };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();

            var consumer = new EvaluateExamConsumer(
                studentOpticalFormRepository,
                logger as ILogger<EvaluateExamConsumer>,
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
