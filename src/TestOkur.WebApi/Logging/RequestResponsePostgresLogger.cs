namespace TestOkur.WebApi.Logging
{
	using System.Threading.Tasks;
	using Dapper;
	using Npgsql;
	using TestOkur.Infrastructure.Mvc;
	using TestOkur.WebApi.Configuration;

	public class RequestResponsePostgresLogger : IRequestResponseLogger
	{
		private readonly string _connectionString;

		public RequestResponsePostgresLogger(ApplicationConfiguration configurationOptions)
		{
			_connectionString = configurationOptions.Postgres;
		}

		public async Task PersistAsync(RequestResponseLog log)
		{
			const string sql = @"INSERT INTO request_response_logs(request,request_datetime_utc,response,response_datetime_utc)
				VALUES(@Request,@RequestDateTimeUtc,@Response,@ResponseDateTimeUtc)";

			using (var connection = new NpgsqlConnection(_connectionString))
			{
				try
				{
					await connection.ExecuteAsync(sql, log);
				}
				catch
				{
					//TODO: Log the exception maybe?
				}
			}
		}
	}
}
