using System.Collections.Generic;

namespace TestOkur.Report.Domain.Optic.Answerkey
{
    public class AnswerKeyOpticalFormSection
    {
        public int LessonId { get; set; }

        public List<QuestionAnswer> Answers { get; set; }
    }
}
