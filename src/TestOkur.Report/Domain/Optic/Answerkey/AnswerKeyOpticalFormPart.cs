namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;

    public class AnswerKeyOpticalFormPart
    {
        public int FormPart { get; set; }

        public List<AnswerKeyOpticalFormSection> Sections { get; set; }
    }
}