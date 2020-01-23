namespace TestOkur.Report.Domain.Optic
{
    public class SectionResult
    {
        public int LessonId { get; set; }

        public int EmptyCount { get; set; }

        public int WrongCount { get; set; }

        public int CorrectCount { get; set; }

        public float Net { get; set; }
    }
}