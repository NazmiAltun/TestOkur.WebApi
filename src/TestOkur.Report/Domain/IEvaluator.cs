namespace TestOkur.Report.Domain
{
    using System.Collections.Generic;
    using TestOkur.Optic.Form;

    public interface IEvaluator
    {
        List<StudentOpticalForm> Evaluate(List<AnswerKeyOpticalForm> answerKeyOpticalForms, List<StudentOpticalForm> forms);
    }
}