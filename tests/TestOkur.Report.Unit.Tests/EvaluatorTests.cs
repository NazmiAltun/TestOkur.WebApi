namespace TestOkur.Report.Unit.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using TestOkur.Optic.Answer;
	using TestOkur.Optic.Form;
	using TestOkur.Optic.Score;
	using TestOkur.Report.Domain;
	using Xunit;

	public class EvaluatorTests
	{
		private readonly Random _random = new Random();

		[Fact]
		public void FillMissingSections_Should_FillMissingSections()
		{
			var answerKeyForms = new List<AnswerKeyOpticalForm>
			{
				GenerateAnswerKeyFormA(),
				GeneratedAnswerKeyFormB(),
			};
			var evaluator = new Evaluator();
			var studentForm = new StudentOpticalForm('A')
			{
				StudentId = 6456,
				StudentNumber = 543,
				StudentFirstName = "Nazmi",
				StudentLastName = "Altun",
				ExamName = "Test-54353",
				ExamDate = DateTime.Today.AddDays(-2),
				ClassroomId = 65,
				Classroom = "11/B",
				SchoolId = 4,
				UserId = "4",
				CityId = 35,
				CityName = "Izmir",
				DistrictId = 20,
				DistrictName = "Bayrakli",
			};
			studentForm.SetFromScanOutput(new ScanOutput("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", 1), answerKeyForms.First());
			evaluator.FillMissingSections(studentForm, answerKeyForms.First());
			studentForm.Sections.Count.Should().Be(answerKeyForms.First().Sections.Count);

			foreach (var section in answerKeyForms.First().Sections)
			{
				studentForm.Sections.First(s => s.LessonId == section.LessonId)
					.Answers.Count().Should().Be(section.Answers.Count);
			}
		}

		[Fact]
		public void ShouldEvaluateExpectedly()
		{
			var answerKeyForms = new List<AnswerKeyOpticalForm>
			{
				GenerateAnswerKeyFormA(),
				GeneratedAnswerKeyFormB(),
			};

			var evaluator = new Evaluator();
			var studentForms = new List<StudentOpticalForm>();
			var studentForm = new StudentOpticalForm('A')
			{
				StudentId = 1,
				StudentNumber = 10512,
				StudentFirstName = "John",
				StudentLastName = "Walker",
				ExamName = "TYT",
				ExamDate = DateTime.Today,
				ClassroomId = 1,
				Classroom = "12/E",
				SchoolId = 1,
				UserId = "1",
				CityId = 35,
				CityName = "Izmir",
				DistrictId = 20,
				DistrictName = "Bayrakli",
			};
			studentForm.SetFromScanOutput(new ScanOutput("EDCBAABDABCADBCCDECEDBDEDEBCEAACBADCCDEADEECBCCBCACEEBADBCDC", 1), answerKeyForms.First());
			studentForm.SetFromScanOutput(new ScanOutput("DDBBDAE BECBBDCAECEDBECCEB DACCDCB A  CDCDDABACDBCAEBEDBCABA", 2), answerKeyForms.First());
			studentForms.Add(studentForm);
			studentForm = new StudentOpticalForm('B')
			{
				StudentId = 2,
				StudentNumber = 10301,
				StudentFirstName = "Mary",
				StudentLastName = "Bloody",
				ExamName = "TYT",
				ExamDate = DateTime.Today,
				ClassroomId = 2,
				Classroom = "12/C",
				UserId = "1",
				SchoolId = 1,
				CityId = 35,
				CityName = "Izmir",
				DistrictId = 20,
				DistrictName = "Bayrakli",
			};
			studentForm.SetFromScanOutput(new ScanOutput("EDCDEAEECEAACBAECCDEAEDCBAABDBBC DBB DECCABDEBCAEBADCEEABDBD", 1), answerKeyForms.Last());
			studentForm.SetFromScanOutput(new ScanOutput("AAECEDBECC BD A       AB                                    ", 2), answerKeyForms.Last());
			studentForms.Add(studentForm);

			studentForms = evaluator.Evaluate(answerKeyForms, studentForms);
			studentForms.First().Should().Match<StudentOpticalForm>(
				x => x.Net == 95 &&
					 x.Score == 416.68f &&
					 Math.Round(x.SuccessPercent) == 79);
			studentForms.Last().Should().Match<StudentOpticalForm>(
				x => x.Net == 60.5f &&
					 x.Score == 301.66f &&
					 Math.Round(x.SuccessPercent) == 50);
			studentForms.First().Orders
				.First(o => o.Name == "NET")
				.Should().Match<StudentOrder>(x =>
					x.GeneralOrder == 1 &&
					x.ClassroomOrder == 1 &&
					x.SchoolOrder == 1 &&
					x.DistrictOrder == 1 &&
					x.CityOrder == 1);
			studentForms.First().Orders
				.First(o => o.Name == answerKeyForms.First().ScoreFormulas.First().ScoreName)
				.Should().Match<StudentOrder>(x =>
					x.GeneralOrder == 1 &&
					x.ClassroomOrder == 1 &&
					x.SchoolOrder == 1 &&
					x.DistrictOrder == 1 &&
					x.CityOrder == 1);
			studentForms.Last().Orders
				.First(o => o.Name == "NET")
				.Should().Match<StudentOrder>(x =>
					x.GeneralOrder == 2 &&
					x.ClassroomOrder == 1 &&
					x.SchoolOrder == 2 &&
					x.DistrictOrder == 2 &&
					x.CityOrder == 2);
			studentForms.First().Sections
				.First(s => s.LessonName == "Tr")
				.Averages.First(a => a.Name == "NET")
				.Should().Match<Average>(
					x => x.City == 35.25f &&
						 x.School == 35.25f &&
						 x.Classroom == 37.5f &&
						 x.District == 35.25f);
		}

		private AnswerKeyOpticalForm GeneratedAnswerKeyFormB()
		{
			var form = GenerateAnswerKeyOpticalForm('B');

			form.AddSection(new AnswerKeyOpticalFormSection(1, "Tr", 40, 1, 1)
			{
				Answers = ParseAnswers("EDBDEDEBCEAACBAECCDEAEDCBAABDABCADBBCDEC"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(2, "Soc", 20, 1, 1)
			{
				Answers = ParseAnswers("CADBEBCAEDBACEEACDBD"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(3, "Math", 40, 2, 1)
			{
				Answers = ParseAnswers("AAECEDBECCEBDDADDCBAAEABECBBDABCCDCEDCEA"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(4, "Sci", 20, 2, 1)
			{
				Answers = ParseAnswers("BEECDDAAEBEDBCAEADBC"),
			});
			return form;
		}

		private AnswerKeyOpticalForm GenerateAnswerKeyFormA()
		{
			var form = GenerateAnswerKeyOpticalForm('A');

			form.AddSection(new AnswerKeyOpticalFormSection(1, "Tr", 40, 1, 1)
			{
				Answers = ParseAnswers("EDCBAABDABCADBBCDECEDBDEDEBCEAACBAECCDEA"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(2, "Soc", 20, 1, 1)
			{
				Answers = ParseAnswers("DBECAEDBCACEEBADBDAC"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(3, "Math", 40, 2, 1)
			{
				Answers = ParseAnswers("DDCBAAEABECBBDAAECEDBECCEBDDACEDCEAABCCD"),
			});
			form.AddSection(new AnswerKeyOpticalFormSection(4, "Sci", 20, 2, 1)
			{
				Answers = ParseAnswers("CDDABEEDBCAEBEDBCAEA"),
			});

			return form;
		}

		private AnswerKeyOpticalForm GenerateAnswerKeyOpticalForm(char booklet)
		{
			return new AnswerKeyOpticalForm
			{
				IncorrectEliminationRate = 4,
				Booklet = booklet,
				ExamName = "TYT",
				ScoreFormulas = new List<ScoreFormula>()
				{
					new ScoreFormula(100, "TYT")
					{
						Coefficients = new List<LessonCoefficient>()
						{
							new LessonCoefficient("Tr", 3.333f),
							new LessonCoefficient("Soc", 3.333f),
							new LessonCoefficient("Math", 3.334f),
							new LessonCoefficient("Sci", 3.334f),
						},
					},
				},
			};
		}

		private List<AnswerKeyQuestionAnswer> ParseAnswers(string answers)
		{
			return answers.Select((t, i) => new AnswerKeyQuestionAnswer(i + 1, t)).ToList();
		}
	}
}
