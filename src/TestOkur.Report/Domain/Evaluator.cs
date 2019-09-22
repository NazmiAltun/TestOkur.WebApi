using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Report.Unit.Tests")]

namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;

    public class Evaluator : IEvaluator
    {
        public List<StudentOpticalForm> JoinSets(List<StudentOpticalForm> firstSet, List<StudentOpticalForm> secondSet)
        {
            foreach (var form in firstSet)
            {
                var matchedForm = secondSet.FirstOrDefault(x => x.StudentId == form.StudentId);

                if (matchedForm != null)
                {
                    form.AddSections(matchedForm.Sections);
                }
            }

            return firstSet;
        }

        public List<StudentOpticalForm> Evaluate(List<AnswerKeyOpticalForm> answerKeyOpticalForms, List<StudentOpticalForm> forms)
        {
            if (forms == null || forms.Count == 0)
            {
                return forms;
            }

            if (answerKeyOpticalForms.Count == 1)
            {
                answerKeyOpticalForms = answerKeyOpticalForms.First().Expand();
            }

            var scoreNames = answerKeyOpticalForms.First().ScoreFormulas
                .Select(s => s.ScoreName).ToList();
            FillMissingSections(answerKeyOpticalForms, forms);
            EvaluateForms(answerKeyOpticalForms, forms);
            SetOrdersAndAverages(scoreNames, forms);
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

        private void FillMissingSections(List<AnswerKeyOpticalForm> answerKeyOpticalForms, List<StudentOpticalForm> forms)
        {
            var answerFormKeyDict = answerKeyOpticalForms
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

        private void SetOrdersAndAverages(List<string> scoreNames, IReadOnlyCollection<StudentOpticalForm> forms)
        {
            var orderLists = CreateOrderLists(scoreNames, forms);
            var netAverageList = new AverageList("NET", forms, s => s.Net);
            var successPercentAverageList = new AverageList("SuccessPercent", forms, s => s.SuccessPercent);

            foreach (var form in forms)
            {
                form.ClearOrders();
                foreach (var orderList in orderLists)
                {
                    form.AddStudentOrder(orderList.GetStudentOrder(form));
                }

                foreach (var section in form.Sections)
                {
                    section.ClearLessonAverages();
                    section.AddLessonAverage(netAverageList.Get(form, section.LessonName));
                    section.AddLessonAverage(successPercentAverageList.Get(form, section.LessonName));
                }
            }

            foreach (var form in forms)
            {
                form.SetAverages(forms);
            }
        }

        private void EvaluateForms(List<AnswerKeyOpticalForm> answerKeyOpticalForms, IEnumerable<StudentOpticalForm> forms)
        {
            var answerFormKeyDict = answerKeyOpticalForms
                .ToDictionary(x => x.Booklet, x => x);

            foreach (var form in forms)
            {
                if (answerFormKeyDict.TryGetValue(form.Booklet, out var answerKeyForm) ||
                    answerFormKeyDict.Count == 1)
                {
                    if (answerFormKeyDict.Count == 1)
                    {
                        answerKeyForm = answerFormKeyDict.First().Value;
                    }

                    form.UpdateCorrectAnswers(answerKeyForm);
                    form.Evaluate(answerKeyForm.IncorrectEliminationRate, answerKeyForm.ScoreFormulas);
                }
            }
        }

        private List<StudentOrderList> CreateOrderLists(
            List<string> scoreNames,
            IReadOnlyCollection<StudentOpticalForm> forms)
        {
            return scoreNames
                .Select(sf => new StudentOrderList(sf.ToUpper(), forms, s => s.Scores[sf.ToUpper()]))
                .Concat(new[] { new StudentOrderList("NET", forms, f => f.Net) })
                .ToList();
        }
    }
}
