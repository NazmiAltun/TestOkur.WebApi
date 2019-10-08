namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using TestOkur.Optic.Form;

    public interface IEvaluator
    {
        IEnumerable<StudentOpticalForm> JoinSets(IEnumerable<StudentOpticalForm> firstSet, IEnumerable<StudentOpticalForm> secondSet);

        IEnumerable<StudentOpticalForm> Evaluate(IReadOnlyCollection<AnswerKeyOpticalForm> answerKeyOpticalForms, IReadOnlyCollection<StudentOpticalForm> forms);

        IEnumerable<SchoolResult> Evaluate(IEnumerable<StudentOpticalForm> forms);
    }
}