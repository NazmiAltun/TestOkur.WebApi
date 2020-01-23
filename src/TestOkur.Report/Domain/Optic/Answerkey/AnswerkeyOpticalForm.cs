namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;

    public class AnswerKeyOpticalForm : OpticalForm
    {
        public byte Booklet { get; set; }
        
        public List<AnswerKeyOpticalFormPart> Parts { get; set; }
    }
}
