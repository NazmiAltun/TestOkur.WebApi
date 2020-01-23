namespace TestOkur.Report.Domain.Optic.Answerkey
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("{ScoreName}")]
    public class ScoreFormula
    {
        public ScoreFormula(float basePoint, string scoreName)
        {
            BasePoint = basePoint;
            ScoreName = scoreName.Replace(".", string.Empty);
            Coefficients = new List<LessonCoefficient>();
        }

        public ScoreFormula()
        {
        }

        public float BasePoint { get; set; }

        public string ScoreName { get; set; }

        public List<LessonCoefficient> Coefficients { get; set; }
    }
}