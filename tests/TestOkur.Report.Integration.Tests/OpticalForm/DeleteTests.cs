namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Events;
    using TestOkur.Report.Integration.Tests.Common;
    using TestOkur.TestHelper;
    using Xunit;

    public class DeleteTests : OpticalFormTest
    {
        [Fact]
        public async Task ShouldDeleteTheForm_And_ReevaluateTheExam()
        {
            var userId = RandomGen.Next(10000);

            using var testServer = Create(userId);
            var client = testServer.CreateClient();

            var forms = new List<StudentOpticalForm>
            {
                new StudentOpticalForm('A')
                {
                    ExamId = RandomGen.Next(),
                    StudentId = RandomGen.Next(),
                    UserId = userId.ToString(),
                    Sections = new List<StudentOpticalFormSection>
                    {
                        new StudentOpticalFormSection(new AnswerKeyOpticalFormSection(1, "TEST")),
                    },
                },
            };
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
