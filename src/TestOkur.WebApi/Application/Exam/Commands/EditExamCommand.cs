namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Optic.Form;

    public class EditExamCommand : CommandBase, IClearCacheWithRegion
    {
        public EditExamCommand(
            Guid id,
            int examId,
            string name,
            DateTime examDate,
            int examTypeId,
            int incorrectEliminationRate,
            string applicableFormTypeCode,
            int answerFormFormat,
            int lessonId,
            int examBookletTypeId,
            IEnumerable<AnswerKeyOpticalForm> answerKeyOpticalForms,
            string notes)
            : base(id)
        {
            ExamId = examId;
            NewName = name;
            NewExamDate = examDate;
            NewExamTypeId = examTypeId;
            NewIncorrectEliminationRate = incorrectEliminationRate;
            NewApplicableFormTypeCode = applicableFormTypeCode;
            NewNotes = notes;
            NewLessonId = lessonId;
            NewExamBookletTypeId = examBookletTypeId;
            NewAnswerFormFormat = answerFormFormat;
            AnswerKeyOpticalForms = answerKeyOpticalForms;
        }

        public EditExamCommand()
        {
        }

        public string Region => Shared ? "Exams" : string.Empty;

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

        public int ExamId { get; set; }

        public DateTime NewExamDate { get; set; }

        public string NewName { get; set; }

        public int NewExamTypeId { get; set; }

        public int NewIncorrectEliminationRate { get; set; }

        public string NewNotes { get; set; }

        public string NewApplicableFormTypeCode { get; set; }

        public int NewAnswerFormFormat { get; set; }

        public int NewLessonId { get; set; }

        public int NewExamBookletTypeId { get; set; }

        public bool Shared { get; set; }

        public IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; set; }
    }
}
