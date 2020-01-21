namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;

    public class AnswerkeyOpticalForm
    {
        public List<AnswerKeyOpticalFormSection> Sections { get; set; }

        public int Part { get; set; }
    }

    public class Answerkey
    {
        public List<AnswerkeyOpticalForm> Forms { get; set; }

        public char Booklet { get; set; }
    }
}
