namespace TestOkur.Report.Infrastructure
{
	using MongoDB.Driver;
	using TestOkur.Infrastructure.Mvc;
	using TestOkur.Optic.Form;
	using TestOkur.Report.Configuration;

	public class TestOkurContext
	{
		private readonly IMongoDatabase _database = null;

		public TestOkurContext(ReportConfiguration configuration)
		{
			var client = new MongoClient(configuration.ConnectionString);
			_database = client.GetDatabase(configuration.Database);
		}

		public IMongoCollection<StudentOpticalForm> StudentOpticalForms =>
			_database.GetCollection<StudentOpticalForm>("StudentOpticalForms");

		public IMongoCollection<AnswerKeyOpticalForm> AnswerKeyOpticalForms =>
			_database.GetCollection<AnswerKeyOpticalForm>("AnswerKeyOpticalForms");

		public IMongoCollection<RequestResponseLog> RequestResponseLogs =>
			_database.GetCollection<RequestResponseLog>("RequestResponseLogs");
	}
}
