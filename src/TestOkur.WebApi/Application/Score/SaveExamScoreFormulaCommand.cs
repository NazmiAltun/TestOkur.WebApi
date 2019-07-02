namespace TestOkur.WebApi.Application.Score
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using TestOkur.Infrastructure.Cqrs;

	[DataContract]
	public class SaveExamScoreFormulaCommand : EditScoreFormulaCommand, IClearCache
	{
		public SaveExamScoreFormulaCommand(
			int examId,
			int originalFormulaId,
			float basePoint,
			Dictionary<int, float> coefficients)
		 : base(default, basePoint, coefficients)
		{
			ExamId = examId;
			OriginalFormulaId = originalFormulaId;
		}

		public IEnumerable<string> CacheKeys => new[] { $"ScoreFormulas_{UserId}" };

		[DataMember]
		public int ExamId { get; private set; }

		[DataMember]
		public int OriginalFormulaId { get; private set; }
	}
}
