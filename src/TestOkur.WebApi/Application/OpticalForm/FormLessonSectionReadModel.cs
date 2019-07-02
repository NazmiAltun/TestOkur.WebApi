namespace TestOkur.WebApi.Application.OpticalForm
{
    public class FormLessonSectionReadModel
    {
        public int LessonId { get; set; }

        public string Lesson { get; set; }

        public int MaxQuestionCount { get; set; }

        public int FormPart { get; set; }

        public int ListOrder { get; set; }
	}
}
