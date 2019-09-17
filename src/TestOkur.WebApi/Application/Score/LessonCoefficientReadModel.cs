namespace TestOkur.WebApi.Application.Score
{
    public class LessonCoefficientReadModel
    {
        public LessonCoefficientReadModel()
        {
        }

        public LessonCoefficientReadModel(string lesson, float coefficient)
        {
            Lesson = lesson;
            Coefficient = coefficient;
        }

        public int LessonCoefficientId { get; set; }

        public int ExamTypeId { get; set; }

        public int LessonId { get; set; }

        public string ExamType { get; set; }

        public string Lesson { get; set; }

        public float Coefficient { get; set; }
    }
}
