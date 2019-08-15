using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Report.Unit.Tests")]

namespace TestOkur.Report.Domain
{
	using System.Collections.Generic;
	using System.Linq;
	using TestOkur.Optic.Form;

	public class Evaluator
	{
		private readonly List<AnswerKeyOpticalForm> _answerKeyOpticalForms;

		public Evaluator(List<AnswerKeyOpticalForm> answerKeyOpticalForms)
		{
			_answerKeyOpticalForms = answerKeyOpticalForms;

			if (_answerKeyOpticalForms.Count == 1)
			{
				_answerKeyOpticalForms = _answerKeyOpticalForms.First().Expand();
			}
		}

		public List<StudentOpticalForm> Evaluate(List<StudentOpticalForm> forms)
		{
			FillMissingSections(forms);
			EvaluateForms(forms);
			SetOrdersAndAverages(forms);
			SetAttendance(forms);

			return forms;
		}

		internal void FillMissingSections(StudentOpticalForm form, AnswerKeyOpticalForm answerKeyForm)
		{
			foreach (var section in answerKeyForm.Sections)
			{
				if (!form.ContainsSection(section.LessonId))
				{
					form.AddEmptySection(section);
				}
			}
		}

		private void FillMissingSections(List<StudentOpticalForm> forms)
		{
			var answerFormKeyDict = _answerKeyOpticalForms
				.ToDictionary(x => x.Booklet, x => x);

			foreach (var form in forms)
			{
				if (answerFormKeyDict.TryGetValue(form.Booklet, out var answerKeyForm))
				{
					FillMissingSections(form, answerKeyForm);
				}
			}
		}

		private void SetAttendance(IReadOnlyCollection<StudentOpticalForm> forms)
		{
			foreach (var form in forms)
			{
				form.SetAttendance(forms);
			}
		}

		private void SetOrdersAndAverages(IReadOnlyCollection<StudentOpticalForm> forms)
		{
			var orderLists = CreateOrderLists(forms);
			var netAverageList = new AverageList("NET", forms, s => s.Net);

			foreach (var form in forms)
			{
				form.ClearOrders();
				foreach (var orderList in orderLists)
				{
					form.AddStudentOrder(orderList.GetStudentOrder(form));
				}

				foreach (var section in form.Sections)
				{
					netAverageList.GetClassroomAverage(section.LessonName, form.ClassroomId);
					section.ClearLessonAverages();
					section.AddLessonAverage(netAverageList.Get(form, section.LessonName));
				}
			}

			foreach (var form in forms)
			{
				form.SetAverages(forms);
			}
		}

		private void EvaluateForms(IEnumerable<StudentOpticalForm> forms)
		{
			var answerFormKeyDict = _answerKeyOpticalForms
				.ToDictionary(x => x.Booklet, x => x);

			foreach (var form in forms)
			{
				if (answerFormKeyDict.TryGetValue(form.Booklet, out var answerKeyForm))
				{
					form.UpdateCorrectAnswers(answerKeyForm);
					form.Evaluate(answerKeyForm.IncorrectEliminationRate, answerKeyForm.ScoreFormulas);
				}
			}
		}

		private List<StudentOrderList> CreateOrderLists(
			IReadOnlyCollection<StudentOpticalForm> forms)
		{
			return _answerKeyOpticalForms.First()
				.ScoreFormulas
				.Select(f => new StudentOrderList(f.ScoreName, forms, s => s.Scores[f.ScoreName]))
				.Concat(new[] { new StudentOrderList("NET", forms, f => f.Net) })
				.ToList();
		}
	}
}
