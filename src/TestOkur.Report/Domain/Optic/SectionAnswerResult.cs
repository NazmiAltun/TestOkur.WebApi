namespace TestOkur.Report.Domain.Optic
{
    public class SectionAnswerResult
    {
        public int LessonId { get; set; }

        public QuestionAnswerResult[] QuestionAnswerResults { get; set; }
    }
}