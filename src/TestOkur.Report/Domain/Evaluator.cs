using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Report.Unit.Tests")]

namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;

    public class Evaluator : IEvaluator
    {
        public IEnumerable<StudentOpticalForm> JoinSets(IEnumerable<StudentOpticalForm> firstSet, IEnumerable<StudentOpticalForm> secondSet)
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

        public IEnumerable<SchoolResult> EvaluateSchoolResults(IEnumerable<StudentOpticalForm> forms)
        {
            var results = forms.GroupBy(
                f => f.SchoolId,
                f => f,
                (schoolId, fs) =>
                    new SchoolResult(fs.First())
                    {
                        ClassroomCount = fs.Select(f => f.ClassroomId).Distinct().Count(),
                        SuccessPercent = fs.Average(f => f.SuccessPercent),
                    }).ToList();
            var orderList = new SchoolOrderList(results, r => r.ScoreAverage);

            foreach (var result in results)
            {
                result.CityOrder = orderList.GetCityOrder(result);
                result.DistrictOrder = orderList.GetDistrictOrder(result);
                result.GeneralOrder = orderList.GetGeneralOrder(result);
            }

            return results;
        }

        public IEnumerable<StudentOpticalForm> Evaluate(IReadOnlyCollection<AnswerKeyOpticalForm> answerKeyOpticalForms, IReadOnlyCollection<StudentOpticalForm> forms)
        {
            if (forms == null || forms.Count == 0)
            {
                return forms;
            }

            if (answerKeyOpticalForms.Count() == 1)
            {
                answerKeyOpticalForms = answerKeyOpticalForms.First().Expand();
            }

            FillMissingSections(answerKeyOpticalForms, forms);
            EvaluateForms(answerKeyOpticalForms, forms);
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

        private void FillMissingSections(IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms, IEnumerable<StudentOpticalForm> forms)
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

        private void SetOrdersAndAverages(IReadOnlyCollection<StudentOpticalForm> forms)
        {
            var orderLists = CreateOrderLists(forms);
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

        private void EvaluateForms(IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms, IEnumerable<StudentOpticalForm> forms)
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

        private List<StudentOrderList> CreateOrderLists(IReadOnlyCollection<StudentOpticalForm> forms)
        {
            var examGrade = forms
                .GroupBy(f => f.Grade, (k, v) => new
                {
                    grade = k,
                    count = v.Count(),
                })
                .OrderByDescending(x => x.count)
                .First().grade;

            var scoreNames = forms
                .Where(f => f.Grade == examGrade)
                .SelectMany(f => f.Scores.Keys)
                .Distinct();

            return scoreNames
                .Select(sf => new StudentOrderList(
                    sf.ToUpper(),
                    forms,
                    s => s.Scores.TryGetValue(sf.ToUpper(), out var val) ? val : 0))
                .Concat(new[] { new StudentOrderList("NET", forms, f => f.Net) })
                .ToList();
        }
    }
}
