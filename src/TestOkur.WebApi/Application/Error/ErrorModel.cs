namespace TestOkur.WebApi.Application.Error
{
	using System.Runtime.Serialization;
	using TestOkur.Contracts;
	using TestOkur.Contracts.Alert;

	[DataContract]
	public class ErrorModel : IntegrationEvent, IUserErrorReceived
	{
		public ErrorModel(
			string reporterEmail,
			int reporterUserId,
			int examId,
			string examName,
			string imageFilePath,
			string description)
		{
			ReporterEmail = reporterEmail;
			ReporterUserId = reporterUserId;
			ExamId = examId;
			ExamName = examName;
			ImageFilePath = imageFilePath;
			Description = description;
		}

		[DataMember]
		public string ReporterEmail { get; private set; }

		[DataMember]
		public int ReporterUserId { get; private set; }

		[DataMember]
		public int ExamId { get; private set; }

		[DataMember]
		public string ExamName { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public string ImageFilePath { get; private set; }
	}
}
