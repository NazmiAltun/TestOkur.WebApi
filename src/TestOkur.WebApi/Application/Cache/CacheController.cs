namespace TestOkur.WebApi.Application.Cache
{
	extern alias signed;

	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using signed::StackExchange.Redis;

	[Route("api/cache")]
	[AllowAnonymous]
	public class CacheController : Controller
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;
		private readonly IConfiguration _configuration;

		public CacheController(
			IConnectionMultiplexer connectionMultiplexer,
			IConfiguration configuration)
		{
			_connectionMultiplexer = connectionMultiplexer;
			_configuration = configuration;
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteAsync()
		{
			foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
			{
				var server = _connectionMultiplexer.GetServer(endPoint);
				await server.FlushAllDatabasesAsync();
			}

			return Ok();
		}
	}
}
