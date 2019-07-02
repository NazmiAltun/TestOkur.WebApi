namespace TestOkur.Report.Integration.Tests.OpticalForm
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TestOkur.Optic.Answer;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Integration.Tests.Common;
    using TestOkur.TestHelper;
    using TestOkur.TestHelper.Extensions;

    public abstract class OpticalFormTest : Test
	{
		protected const string ApiPath = "api/v1/forms";
		private const string Booklets = "ABCD";
		private const string Answers = "ABCDE";

		protected async Task<IEnumerable<TOpticalForm>> GetListAsync<TOpticalForm>(HttpClient client, int examId)
			where TOpticalForm : OpticalForm
		{
			var path = $"{ApiPath}/exam/{GetSubPath<TOpticalForm>()}/{examId}";

			var response = await client.GetAsync(path);
			return await response.ReadAsync<IEnumerable<TOpticalForm>>();
		}

		protected IEnumerable<AnswerKeyOpticalForm> GenerateAnswerKeyOpticalForms(int count, int lessonId = 1, string lessonName = "Test")
		{
			var booklets = new List<char>(Booklets);

			for (var i = 0; i < count && booklets.Count > 0; i++)
			{
				yield return new AnswerKeyOpticalForm()
				{
					Booklet = booklets.First(),
					Sections = new List<AnswerKeyOpticalFormSection>()
					{
						new AnswerKeyOpticalFormSection(lessonId, lessonName)
						{
							Answers = GenerateAnswerKeyQuestionAnswers(100).ToList(),
						},
					},
				};
				booklets.RemoveAt(0);
			}
		}

		protected StudentOpticalForm GenerateStudentForm(int examId, int userId, int lessonId = 1, string lessonName = "Test")
		{
			return new StudentOpticalForm()
			{
				UserId = userId.ToString(),
				SchoolId = userId,
				Booklet = Booklets.Random(),
				ExamId = examId,
				StudentId = RandomGen.Next(),
				StudentNumber = RandomGen.Next(),
				Sections = new List<StudentOpticalFormSection>()
				{
					new StudentOpticalFormSection(lessonId, lessonName)
					{
						Answers = GenerateAnswers(160).ToList(),
					},
				},
			};
		}

		protected IEnumerable<QuestionAnswer> GenerateAnswers(int count)
		{
			for (var i = 0; i < count; i++)
			{
				yield return GenerateAnswer();
			}
		}

		private string GetSubPath<TOpticalForm>()
			where TOpticalForm : OpticalForm =>
			typeof(TOpticalForm) == typeof(StudentOpticalForm) ? "student" : "answer";

		private QuestionAnswer GenerateAnswer()
		{
			return new QuestionAnswer()
			{
				Answer = Answers.Random(),
				QuestionNo = RandomGen.Next(100),
				SubjectName = RandomGen.String(250),
				SubjectId = RandomGen.Next(),
			};
		}

		private IEnumerable<AnswerKeyQuestionAnswer> GenerateAnswerKeyQuestionAnswers(int count)
		{
			for (var i = 0; i < count; i++)
			{
				yield return GenerateAnswerKeyQuestionAnswer();
			}
		}

		private AnswerKeyQuestionAnswer GenerateAnswerKeyQuestionAnswer()
		{
			return new AnswerKeyQuestionAnswer()
			{
				Answer = "ABCDE".Random(),
				QuestionNo = RandomGen.Next(100),
				SubjectName = RandomGen.String(250),
				SubjectId = RandomGen.Next(),
				QuestionNoBookletB = RandomGen.Next(100),
				QuestionNoBookletC = RandomGen.Next(100),
				QuestionNoBookletD = RandomGen.Next(100),
				QuestionAnswerCancelAction = QuestionAnswerCancelAction.None,
			};
		}
	}
}
