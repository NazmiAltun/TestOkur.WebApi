namespace TestOkur.WebApi.Application.Score
{
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class EditScoreFormulaCommand : CommandBase
    {
        public EditScoreFormulaCommand(
            int scoreFormulaId,
            float basePoint,
            Dictionary<int, float> coefficients)
        {
            ScoreFormulaId = scoreFormulaId;
            BasePoint = basePoint;
            Coefficients = coefficients;
        }

        public EditScoreFormulaCommand()
        {
            Coefficients = new Dictionary<int, float>();
        }

        public int ScoreFormulaId { get; set; }

        public float BasePoint { get; set; }

        public Dictionary<int, float> Coefficients { get; set; }
    }
}
