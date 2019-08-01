namespace TestOkur.Contracts.Alert
{
	public interface IUserErrorReceived
	{
		string ReporterEmail { get; }

		int ReporterUserId { get; }

		int ExamId { get; }

		string ExamName { get; }

		string ImageFilePath { get; }

		string Description { get; }
	}
}
