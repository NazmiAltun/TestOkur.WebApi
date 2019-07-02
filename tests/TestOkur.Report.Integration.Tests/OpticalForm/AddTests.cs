namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Optic.Form;
    using TestOkur.TestHelper;
    using Xunit;

    public class AddTests : OpticalFormTest
	{
		[Fact]
		public async Task When_FormsExists_Then_ShouldReplaceStudentOpticalForms()
		{
			var userId = RandomGen.Next(10000);
			using (var testServer = Create(userId))
			{
				var examId = RandomGen.Next();
				var client = testServer.CreateClient();
				var forms = new List<StudentOpticalForm>
				{
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
				};
				var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
				response.EnsureSuccessStatusCode();
				forms.First().Sections.Clear();
				forms.First().Sections.Add(new StudentOpticalFormSection(
					2065, "TestLesson")
					{
						Answers = GenerateAnswers(10).ToList(),
					});
				forms.Last().Sections.Clear();
				forms.Last().Sections.Add(new StudentOpticalFormSection(
					2065, "TestLesson")
				{
					Answers = GenerateAnswers(10).ToList(),
				});
				response = await client.PostAsync(ApiPath, forms.ToJsonContent());
				response.EnsureSuccessStatusCode();
				var examForms = await GetListAsync<StudentOpticalForm>(client, examId);
				examForms.Should().HaveCount(2);
				examForms.Should().Contain(
					f => f.Sections.First().LessonName == "TestLesson");
			}
		}

		[Fact]
		public async Task ShouldAddStudentOpticalForms()
		{
			var userId = RandomGen.Next(10000);
			using (var testServer = Create(userId))
			{
				var examId = RandomGen.Next();
				var client = testServer.CreateClient();
				var forms = new List<StudentOpticalForm>
				{
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
					GenerateStudentForm(examId, userId),
				};
				var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
				response.EnsureSuccessStatusCode();
				var examForms = await GetListAsync<StudentOpticalForm>(client, examId);
				examForms.Select(e => e.StudentNumber)
					.Should().BeEquivalentTo(forms.Select(e => e.StudentNumber));
				examForms.Select(e => e.ExamId)
					.Should().BeEquivalentTo(forms.Select(e => e.ExamId));
				examForms.Select(e => e.Booklet)
					.Should().BeEquivalentTo(forms.Select(e => e.Booklet));
			}
		}
	}
}
