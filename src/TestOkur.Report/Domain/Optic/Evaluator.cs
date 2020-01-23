namespace TestOkur.Report.Domain.Optic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestOkur.Report.Domain.Optic.Answerkey;
    using TestOkur.Report.Domain.Statistics;

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

            var incorrectEliminationRate = answerKeyOpticalForms.First().IncorrectEliminationRate;

            foreach (var studentOpticalForm in studentOpticalForms)
            {
                var studentSectionAnswerResult = CalculateStudentSectionResults(answerKeyOpticalForms, studentOpticalForm);
                result.StudentSectionAnswerResults.Add(studentSectionAnswerResult);
                result.StudentExamResults.Add(new StudentExamResult(
                    studentOpticalForm.ExamId,
                    studentOpticalForm.StudentId,
                    studentOpticalForm.ClassroomId,
                    studentOpticalForm.SchoolId,
                    studentOpticalForm.DistrictId,
                    studentOpticalForm.CityId,
                    studentSectionAnswerResult.ToSectionResults(incorrectEliminationRate)));
            }

            result.ExamStatistics = StatisticsCalculator.Calculate(result.StudentExamResults);

            return result;
        }

        public static StudentSectionAnswerResult CalculateStudentSectionResults(
            List<AnswerKeyOpticalForm> answerKeyOpticalForms,
            StudentOpticalForm studentOpticalForm)
        {
            var result = new StudentSectionAnswerResult(studentOpticalForm.StudentId);

            foreach (var scanResult in studentOpticalForm.ScanResults)
            {
                var sections = answerKeyOpticalForms.First(a => a.Booklet == scanResult.Booklet)
                    .Parts.First(p => p.FormPart == scanResult.FormPart)
                    .Sections;

                var markIndex = 0;

                foreach (var section in sections)
                {
                    var studentSectionAnswers = scanResult.Answers.AsSpan().Slice(markIndex, section.MaxQuestionCount);
                    result.SectionAnswerResults.Add(GetQuestionAnswerResults(section, studentSectionAnswers));
                    markIndex += section.MaxQuestionCount;
                }
            }

            return result;
        }

        public static SectionAnswerResult GetQuestionAnswerResults(AnswerKeyOpticalFormSection section, Span<byte> studentSectionAnswers)
        {
            var questionAnswerResults = new QuestionAnswerResult[section.Answers.Count];

            for (var i = 0; i < section.Answers.Count; i++)
            {
                if (section.Answers[i].QuestionAnswerCancelAction == QuestionAnswerCancelAction.EmptyForAll ||
                    Answers.IsEmpty(studentSectionAnswers[i]))
                {
                    questionAnswerResults[i] = QuestionAnswerResult.Empty;
                }
                else if (section.Answers[i].QuestionAnswerCancelAction == QuestionAnswerCancelAction.CorrectForAll ||
                         section.Answers[i].Answer == studentSectionAnswers[i])
                {
                    questionAnswerResults[i] = QuestionAnswerResult.Correct;
                }
                else if (!Answers.IsValid(studentSectionAnswers[i]))
                {
                    questionAnswerResults[i] = QuestionAnswerResult.Invalid;
                }
                else
                {
                    questionAnswerResults[i] = QuestionAnswerResult.Wrong;
                }
            }

            return new SectionAnswerResult(section.LessonId, questionAnswerResults);
        }
    }
}
