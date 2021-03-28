namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Events;
    using TestOkur.Report.Integration.Tests.Common;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class DeleteTests : OpticalFormTest , IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public DeleteTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldDeleteTheForm_And_ReevaluateTheExam(int userId, StudentOpticalForm sform)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(userId);

            sform.UserId = userId.ToString();
            var forms = new List<StudentOpticalForm> { sform };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var form = (await GetListAsync<StudentOpticalForm>(
                client, forms.First().ExamId)).First();
            await client.DeleteAsync($"{ApiPath}/{form.Id}");
            form = (await GetListAsync<StudentOpticalForm>(
                client, forms.First().ExamId)).FirstOrDefault();
            form.Should().BeNull();
            var events = Consumer.Instance.GetAll<IEvaluateExam>().ToList();
            events.Should().Contain(e => e.ExamId == forms.First().ExamId);
        }
    }
}
