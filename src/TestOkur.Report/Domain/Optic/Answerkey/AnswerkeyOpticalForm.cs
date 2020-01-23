namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;

    public class AnswerKeyOpticalForm : OpticalForm
    {
        public byte Booklet { get; set; }

        public int IncorrectEliminationRate { get; set; }

        public List<AnswerKeyOpticalFormPart> Parts { get; set; }

        public List<ScoreFormula> ScoreFormulas { get; set; }
    }
}
