namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Optic.Form;

    public class CreateExamCommand : CommandBase, IClearCacheWithRegion
    {
        public CreateExamCommand(
            Guid id,
            string name,
            DateTime examDate,
            int examTypeId,
            int incorrectEliminationRate,
            string applicableFormTypeCode,
            int answerFormFormat,
            int lessonId,
            int examBookletTypeId,
            IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms,
            string notes,
            bool shared = false)
            : base(id)
        {
            Name = name;
            AnswerFormFormat = answerFormFormat;
            LessonId = lessonId;
            ExamDate = examDate;
            ExamTypeId = examTypeId;
            IncorrectEliminationRate = incorrectEliminationRate;
            ApplicableFormTypeCode = applicableFormTypeCode;
            ExamBookletTypeId = examBookletTypeId;
            AnswerKeyOpticalForms = answerKeyOpticalForms;
            Notes = notes;
            Shared = shared;
        }

        public CreateExamCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

        public string Region => Shared ? "Exams" : string.Empty;

        public DateTime ExamDate { get; set; }

        public string Name { get; set; }

        public int ExamTypeId { get; set; }

        public int IncorrectEliminationRate { get; set; }

        public string Notes { get; set; }

        public string ApplicableFormTypeCode { get; set; }

        public int AnswerFormFormat { get; set; }

        public int LessonId { get; set; }

        public int ExamBookletTypeId { get; set; }

        public IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; set; }

        public bool Shared { get; set; }
    }
}
