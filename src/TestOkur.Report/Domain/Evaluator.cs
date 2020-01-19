using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestOkur.Report.Unit.Tests")]

namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain.Statistics;

    public class Evaluator : IEvaluator
    {
        public IEnumerable<StudentOpticalForm> JoinSets(IEnumerable<StudentOpticalForm> firstSet, IEnumerable<StudentOpticalForm> secondSet)
        {
            return firstSet.Join(
                    secondSet,
                    first => first.StudentId,
                    second => second.StudentId,
                    (first, second) => first.Merge(second))
                .ToList();
        }

        public IEnumerable<SchoolResult> EvaluateSchoolResults(
            ExamStatistics examStatistics,
            IEnumerable<StudentOpticalForm> forms)
        {
            var sections = forms
                .OrderByDescending(f => f.Sections.Count)
                .First()
                .Sections;
            var results = forms.GroupBy(
                f => f.SchoolId,
                f => f,
                (schoolId, fs) => new SchoolResult(examStatistics, fs, sections)).ToList();
            var orderList = new SchoolOrderList(results, r => r.ScoreAverage);
            var sectionOrderList = new Dictionary<string, SchoolOrderList>();

            for (var i = 0; i < sections.Count; i++)
            {
                var index = i;
                sectionOrderList.Add(sections[i].LessonName, new SchoolOrderList(results, r => r.Sections[index].Net));
            }

            foreach (var result in results)
            {
                result.CityOrder = orderList.GetCityOrder(result);
                result.DistrictOrder = orderList.GetDistrictOrder(result);
                result.GeneralOrder = orderList.GetGeneralOrder(result);

                foreach (var section in result.Sections)
                {
                    section.CityOrder = sectionOrderList[section.LessonName].GetCityOrder(result);
                    section.DistrictOrder = sectionOrderList[section.LessonName].GetDistrictOrder(result);
                    section.GeneralOrder = sectionOrderList[section.LessonName].GetGeneralOrder(result);
                }
            }

            return results;
        }

        public IEnumerable<StudentOpticalForm> Evaluate(IReadOnlyCollection<AnswerKeyOpticalForm> answerKeyOpticalForms, IReadOnlyCollection<StudentOpticalForm> forms)
        {
            if (forms == null || forms.Count == 0)
            {
                return forms;
            }

            if (answerKeyOpticalForms.Count == 1)
            {
                answerKeyOpticalForms = answerKeyOpticalForms.First().Expand();
            }

            FillMissingSections(answerKeyOpticalForms, forms);
            EvaluateForms(answerKeyOpticalForms, forms);
            SetOrder(forms);
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

        private void SetOrder(IReadOnlyCollection<StudentOpticalForm> forms)
        {
            var orderLists = CreateOrderLists(forms);

            foreach (var form in forms)
            {
                form.ClearOrders();
                foreach (var orderList in orderLists)
                {
                    form.AddStudentOrder(orderList.GetStudentOrder(form));
                }
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
