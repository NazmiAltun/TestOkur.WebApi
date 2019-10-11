namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Optic.Form;

    [DataContract]
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

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

        public string Region => Shared ? "Exams" : string.Empty;

        [DataMember]
        public DateTime ExamDate { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int ExamTypeId { get; private set; }

        [DataMember]
        public int IncorrectEliminationRate { get; private set; }

        [DataMember]
        public string Notes { get; private set; }

        [DataMember]
        public string ApplicableFormTypeCode { get; private set; }

        [DataMember]
        public int AnswerFormFormat { get; private set; }

        [DataMember]
        public int LessonId { get; private set; }

        [DataMember]
        public int ExamBookletTypeId { get; private set; }

        [DataMember]
        public IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; private set; }

        [DataMember]
        public bool Shared { get; set; }
    }
}
