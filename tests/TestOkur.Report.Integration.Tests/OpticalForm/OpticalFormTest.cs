namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using AutoFixture;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Optic.Answer;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Integration.Tests.Common;
    using TestOkur.TestHelper.Extensions;

    public abstract class OpticalFormTest : Test
    {
        protected const string ApiPath = "api/v1/forms";
        private const string Booklets = "ABCD";

        protected async Task<IEnumerable<TOpticalForm>> GetListAsync<TOpticalForm>(HttpClient client, int examId)
            where TOpticalForm : OpticalForm
        {
            var path = $"{ApiPath}/exam/{GetSubPath<TOpticalForm>()}/{examId}";

            var response = await client.GetAsync(path);
            return await response.ReadAsync<IEnumerable<TOpticalForm>>();
        }

        protected async Task<IEnumerable<StudentOpticalForm>> GetStudentFormsByStudentIdAsync(
            HttpClient client, int studentId)
        {
            var response = await client.GetAsync($"{ApiPath}/student/{studentId}");
            return await response.ReadAsync<IEnumerable<StudentOpticalForm>>();
        }

        protected IEnumerable<AnswerKeyOpticalForm> GenerateAnswerKeyOpticalForms(IFixture fixture, int count, int lessonId = 1, string lessonName = "Test")
        {
            var booklets = new List<char>(Booklets);
            var answers = new List<AnswerKeyQuestionAnswer>();

            for (var i = 0; i < 100; i++)
            {
                answers.Add(fixture.Create<AnswerKeyQuestionAnswer>());
            }

            for (var i = 0; i < count && booklets.Count > 0; i++)
            {
                yield return new AnswerKeyOpticalForm()
                {
                    Booklet = booklets.First(),
                    Sections = new List<AnswerKeyOpticalFormSection>()
                    {
                        new AnswerKeyOpticalFormSection(lessonId, lessonName)
                        {
                            Answers = answers,
                        },
                    },
                };
                booklets.RemoveAt(0);
            }
        }

        protected StudentOpticalForm GenerateStudentForm(IFixture fixture, int examId, int userId, int lessonId = 1, string lessonName = "Test")
        {
            var form = fixture.Create<StudentOpticalForm>();
            form.UserId = userId.ToString();
            form.SchoolId = userId;
            form.ExamId = examId;
            form.Sections.First().LessonName = lessonName;
            form.Sections.First().LessonId = lessonId;

            return form;
        }

        private string GetSubPath<TOpticalForm>()
            where TOpticalForm : OpticalForm =>
            typeof(TOpticalForm) == typeof(StudentOpticalForm) ? "student" : "answer";

    }
}
