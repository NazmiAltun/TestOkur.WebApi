using System.Linq;

namespace TestOkur.Report.Domain.Optic
{
    using System.Collections.Generic;
    using TestOkur.Report.Domain.Optic.Answerkey;

    public static class Evaluator
    {
        public static EvaluationResult Evaluate(
            List<AnswerKeyOpticalForm> answerKeyOpticalForms,
            List<StudentOpticalForm> studentOpticalForms)
        {
            var result = new EvaluationResult();

            if (studentOpticalForms == null || studentOpticalForms.Count == 0)
            {
                return result;
            }

            return result;
        }

        public static StudentSectionAnswerResult CalculateStudentSectionResults(
            List<AnswerKeyOpticalForm> answerKeyOpticalForms,
            StudentOpticalForm studentOpticalForm)
        {
            var result = new StudentSectionAnswerResult(studentOpticalForm.StudentId);

            foreach (var scanResult in studentOpticalForm.ScanResults)
            {

            }

            return result;
        }

        public static List<AnswerKeyOpticalFormSection> FindSections(
            List<AnswerKeyOpticalForm> answerKeyOpticalForms,
            byte booklet,
            int formPart)
        {
            var form = answerKeyOpticalForms.FirstOrDefault(f => f.Booklet == booklet);

            if (form != null)
            {
                return form
                    .Parts.First(p => p.FormPart == formPart)
                    .Sections;
            }
        }
    }
}
