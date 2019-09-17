namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using TestOkur.Optic.Answer;
    using TestOkur.Optic.Form;
    using TestOkur.Optic.Score;
    using TestOkur.TestHelper;
    using Xunit;

    public class AddTests : OpticalFormTest
	{
		[Fact]
		public async Task FormWithScoreShouldBeAdded()
		{
			var scoreFormula = new ScoreFormula(100, "5. Grade");
			scoreFormula.Coefficients.Add(new LessonCoefficient("Turkish", 3.333f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Social Science", 3.333f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Basic Mathematics", 3.334f));
			scoreFormula.Coefficients.Add(new LessonCoefficient("Science", 3.334f));
			var answerKeyForm = new AnswerKeyOpticalForm(
				'A',
				new List<ScoreFormula> { scoreFormula });
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(1, "Turkish", 40, 1, 1)
			{
				Answers = ParseAnswers("CECBEEBADEEBACBDBBEDDAEDDACEABDEDCBDBABD"),
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(6, "Social Science", 20, 1, 2)
			{
				Answers = ParseAnswers("EAEADADECEEDCDEAEDBA"),
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(5, "Basic Mathematics", 40, 2, 3)
			{
				Answers = ParseAnswers("DDBECACAACBCBECAEAADCDCDEDABAACCBDBAEDCB"),
			});
			answerKeyForm.AddSection(new AnswerKeyOpticalFormSection(2, "Science", 20, 2, 4)
			{
				Answers = ParseAnswers("DBCCEEBAEECAACBBDECE"),
			});
			var studentForm = new StudentOpticalForm('A');
			studentForm.SetFromScanOutput(new ScanOutput("CE EEE       C   EA DAE  ADEBEDEDCADEBAEDBE     D  B DEAEAAA", 1), answerKeyForm);
			studentForm.SetFromScanOutput(new ScanOutput("CB                                      A     CE  D         ", 2), answerKeyForm);
			studentForm.Evaluate(4, answerKeyForm.ScoreFormulas);

			var userId = RandomGen.Next(10000);
			studentForm.UserId = userId.ToString();
			studentForm.ExamId = RandomGen.Next();

			using (var testServer = Create(userId))
			{
				var client = testServer.CreateClient();

				var forms = new List<StudentOpticalForm>
				{
					studentForm,
				};
				var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
				response.EnsureSuccessStatusCode();
			}
		}

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
				 new AnswerKeyOpticalFormSection(2065, "TestLesson"))
					{
						Answers = GenerateAnswers(10).ToList(),
					});
				forms.Last().Sections.Clear();
				forms.Last().Sections.Add(new StudentOpticalFormSection(
					new AnswerKeyOpticalFormSection(2065, "TestLesson"))
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

		private List<AnswerKeyQuestionAnswer> ParseAnswers(string answers)
		{
			return answers.Select((t, i) => new AnswerKeyQuestionAnswer(i + 1, t)).ToList();
		}
	}
}
