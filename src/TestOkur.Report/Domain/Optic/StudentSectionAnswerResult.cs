namespace TestOkur.Report.Domain.Optic
{
    using System.Collections.Generic;

    public class StudentSectionAnswerResult
    {
        public StudentSectionAnswerResult(int studentId)
            : this()
        {
            StudentId = studentId;
        }

        private StudentSectionAnswerResult()
        {
            SectionAnswerResults = new List<SectionAnswerResult>();
        }

        public int StudentId { get; set; }

        public List<SectionAnswerResult> SectionAnswerResults { get; set; }

        public SectionResult[] ToSectionResults(int incorrectEliminationRate)
        {
            var sectionResults = new SectionResult[SectionAnswerResults.Count];

            for (var i = 0; i < SectionAnswerResults.Count; i++)
            {
                var emptyCount = 0;
                var correctCount = 0;
                var wrongCount = 0;

                foreach (var questionAnswerResult in SectionAnswerResults[i].QuestionAnswerResults)
                {
                    if (questionAnswerResult == QuestionAnswerResult.Empty)
                    {
                        emptyCount++;
                    }
                    else if (questionAnswerResult == QuestionAnswerResult.Correct)
                    {
                        correctCount++;
                    }
                    else
                    {
                        wrongCount++;
                    }
                }

                var net = incorrectEliminationRate == 0
                    ? correctCount
                    : correctCount - ((float)wrongCount / incorrectEliminationRate);
                sectionResults[i] = new SectionResult(
                    SectionAnswerResults[i].LessonId,
                    emptyCount,
                    wrongCount,
                    correctCount,
                    net);
            }

            return sectionResults;
        }
    }
}