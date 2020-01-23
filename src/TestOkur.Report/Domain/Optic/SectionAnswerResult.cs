namespace TestOkur.Report.Domain.Optic
{
    public class SectionAnswerResult
    {
        public SectionAnswerResult(int lessonId, QuestionAnswerResult[] questionAnswerResults)
        {
            LessonId = lessonId;
            QuestionAnswerResults = questionAnswerResults;
        }

        private SectionAnswerResult()
        {
        }

        public int LessonId { get; set; }

        public QuestionAnswerResult[] QuestionAnswerResults { get; set; }
    }
}