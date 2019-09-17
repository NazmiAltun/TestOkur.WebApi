namespace TestOkur.WebApi.Application.Exam.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.Cqrs;
    using TestOkur.Optic.Form;

    [DataContract]
    public class EditExamCommand : CommandBase, IClearCache
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

		public IEnumerable<string> CacheKeys => new[] { $"Exams_{UserId}" };

		[DataMember]
		public int ExamId { get; private set; }

		[DataMember]
		public DateTime NewExamDate { get; private set; }

		[DataMember]
		public string NewName { get; private set; }

		[DataMember]
		public int NewExamTypeId { get; private set; }

		[DataMember]
		public int NewIncorrectEliminationRate { get; private set; }

		[DataMember]
		public string NewNotes { get; private set; }

		[DataMember]
		public string NewApplicableFormTypeCode { get; private set; }

		[DataMember]
		public int NewAnswerFormFormat { get; private set; }

		[DataMember]
		public int NewLessonId { get; private set; }

		[DataMember]
		public int NewExamBookletTypeId { get; private set; }

		[DataMember]
		public IEnumerable<AnswerKeyOpticalForm> AnswerKeyOpticalForms { get; private set; }
	}
}
