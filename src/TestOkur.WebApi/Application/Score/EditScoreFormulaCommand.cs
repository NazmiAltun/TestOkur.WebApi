namespace TestOkur.WebApi.Application.Score
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using TestOkur.Infrastructure.Cqrs;

	[DataContract]
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

		[DataMember]
		public int ScoreFormulaId { get; private set; }

		[DataMember]
		public float BasePoint { get; private set; }

		[DataMember]
		public Dictionary<int, float> Coefficients { get; private set; }
			= new Dictionary<int, float>();
	}
}
