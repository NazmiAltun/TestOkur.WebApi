using TestOkur.Report.Integration.Tests.Common;

namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using AutoFixture;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Optic.Answer;
    using TestOkur.Optic.Form;
    using TestOkur.Optic.Score;
    using TestOkur.Serialization;
    using TestOkur.Test.Common;
    using Xunit;

    public class AddTests : OpticalFormTest, IClassFixture<WebApplicationFactory>
    {
        private readonly WebApplicationFactory _webApplicationFactory;

        public AddTests(WebApplicationFactory webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task FormWithScoreShouldBeAdded(int examId, int userId)
        {
            var scoreFormula = new ScoreFormula(100, "5. Grade", 5);
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
            studentForm.SetFromScanOutput(new ScanOutput("CE EEE       C   EA DAE  ADEBEDEDCADEBAEDBE     D  B DEAEAAA", 1, 0, 'A'), answerKeyForm);
            studentForm.SetFromScanOutput(new ScanOutput("CB                                      A     CE  D         ", 2, 0, 'A'), answerKeyForm);
            studentForm.Evaluate(4, answerKeyForm.ScoreFormulas);

            studentForm.UserId = userId.ToString();
            studentForm.ExamId = examId;

            var client = _webApplicationFactory.CreateClientWithUserId(userId);

            var forms = new List<StudentOpticalForm>
            {
                studentForm,
            };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task When_FormsExists_Then_ShouldReplaceStudentOpticalForms(IFixture fixture, int userId, StudentOpticalFormSection section, int examId)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var forms = new List<StudentOpticalForm>
                {
                    GenerateStudentForm(fixture,examId, userId),
                    GenerateStudentForm(fixture,examId, userId),
                };
            var response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            forms.First().Sections.Clear();
            forms.First().Sections.Add(section);
            forms.Last().Sections.Clear();
            forms.Last().Sections.Add(section);
            response = await client.PostAsync(ApiPath, forms.ToJsonContent());
            response.EnsureSuccessStatusCode();
            var examForms = await GetListAsync<StudentOpticalForm>(client, examId);
            examForms.Should().HaveCount(2);
            examForms.Should().Contain(
                f => f.Sections.First().LessonName == forms.First().Sections.First().LessonName);
        }

        [Theory(Skip = "Fix it later")]
        [TestOkurAutoData]
        public async Task ShouldAddStudentOpticalForms(IFixture fixture, int userId, int examId)
        {
            var client = _webApplicationFactory.CreateClientWithUserId(userId);
            var forms = new List<StudentOpticalForm>
            {
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
                GenerateStudentForm(fixture,examId, userId),
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

        private List<AnswerKeyQuestionAnswer> ParseAnswers(string answers)
        {
            return answers.Select((t, i) => new AnswerKeyQuestionAnswer(i + 1, t)).ToList();
        }
    }
}
