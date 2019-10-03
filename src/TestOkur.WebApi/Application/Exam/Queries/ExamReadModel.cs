namespace TestOkur.WebApi.Application.Exam.Queries
{
    using System;

    public class ExamReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ExamTypeId { get; set; }

        public string ExamTypeName { get; set; }

        public int IncorrectEliminationRate { get; set; }

        public DateTime ExamDate { get; set; }

        public string Notes { get; set; }

        public string ApplicableFormTypeCode { get; set; }

        public int AnswerFormFormatId { get; set; }

        public int LessonId { get; set; }

        public string LessonName { get; set; }

        public int ExamBookletTypeId { get; set; }

        public bool Shared { get; set; }
    }
}
