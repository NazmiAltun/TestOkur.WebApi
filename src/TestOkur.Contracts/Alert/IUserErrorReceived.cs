namespace TestOkur.Contracts.Alert
{
	public interface IUserErrorReceived
	{
		int ReporterUserId { get; }

		int ExamId { get; }

		string ExamName { get; }

		string UserEmail { get; }

		string UserPhone { get; }

		string Image1FilePath { get; }

		string Image2FilePath { get; }

		string Image3FilePath { get; }

		string Description { get; }
	}
}
