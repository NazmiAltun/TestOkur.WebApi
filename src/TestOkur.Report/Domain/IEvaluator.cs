namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using TestOkur.Optic.Form;
    using TestOkur.Report.Domain.Statistics;

    public interface IEvaluator
    {
        IEnumerable<StudentOpticalForm> JoinSets(IEnumerable<StudentOpticalForm> firstSet, IEnumerable<StudentOpticalForm> secondSet);

        IEnumerable<StudentOpticalForm> Evaluate(IReadOnlyCollection<AnswerKeyOpticalForm> answerKeyOpticalForms, IReadOnlyCollection<StudentOpticalForm> forms);

        IEnumerable<SchoolResult> EvaluateSchoolResults(ExamStatistics examStatistics, IEnumerable<StudentOpticalForm> forms);
    }
}