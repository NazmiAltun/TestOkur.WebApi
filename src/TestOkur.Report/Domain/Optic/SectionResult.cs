namespace TestOkur.Report.Domain.Optic
{
    public class SectionResult
    {
        public SectionResult(int lessonId, int emptyCount, int wrongCount, int correctCount, float net)
        {
            LessonId = lessonId;
            EmptyCount = emptyCount;
            WrongCount = wrongCount;
            CorrectCount = correctCount;
            Net = net;
        }

        private SectionResult()
        {
        }

        public int LessonId { get; set; }

        public int EmptyCount { get; set; }

        public int WrongCount { get; set; }

        public int CorrectCount { get; set; }

        public int QuestionCount => WrongCount + CorrectCount + EmptyCount;

        public float Net { get; set; }
    }
}