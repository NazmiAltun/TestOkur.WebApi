namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;

    public class AnswerKeyOpticalFormSection
    {
        public int LessonId { get; set; }

        public int MaxQuestionCount { get; set; }

        public List<QuestionAnswer> Answers { get; set; }
    }
}
