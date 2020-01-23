namespace TestOkur.Report.Domain.Optic.Answerkey
{
    public class LessonCoefficient
    {
        public LessonCoefficient(int lessonId, float coefficient)
        {
            LessonId = lessonId;
            Coefficient = coefficient;
        }

        public LessonCoefficient()
        {
        }

        public int LessonId { get; set; }

        public float Coefficient { get; set; }
    }
}