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
			string image1FilePath,
			string image2FilePath,
			string image3FilePath,
			string description)
		{
			ReporterEmail = reporterEmail;
			ReporterUserId = reporterUserId;
			ExamId = examId;
			ExamName = examName;
			Image1FilePath = image1FilePath;
			Image2FilePath = image2FilePath;
			Image3FilePath = image3FilePath;
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
		public string Image1FilePath { get; private set; }

		[DataMember]
		public string Image2FilePath { get; private set; }

		[DataMember]
		public string Image3FilePath { get; private set; }
	}
}
