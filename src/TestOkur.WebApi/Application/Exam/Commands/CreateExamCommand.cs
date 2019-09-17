namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.Optic.Form;

    [DataContract]
    public class CreateExamCommand : CommandBase, IClearCache
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
            string notes)
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
        }

        public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

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
    }
}
