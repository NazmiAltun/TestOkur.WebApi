namespace TestOkur.Contracts.Alert
{
	public interface IUserErrorReceived
	{
		string ReporterEmail { get; }

		int ReporterUserId { get; }

		int ExamId { get; }

		string ExamName { get; }

		string Image1FilePath { get; }

		string Image2FilePath { get; }

		string Image3FilePath { get; }

		string Description { get; }
	}
}
