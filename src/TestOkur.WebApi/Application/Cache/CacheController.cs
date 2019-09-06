﻿namespace TestOkur.WebApi.Application.Cache
{
	extern alias signed;

	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using IConnectionMultiplexer = signed::StackExchange.Redis.IConnectionMultiplexer;

	[Route("api/cache")]
	[AllowAnonymous]
	public class CacheController : ControllerBase
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;

		public CacheController(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer;
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteAsync()
		{
			foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
			{
				var server = _connectionMultiplexer.GetServer(endPoint);
				await server.FlushAllDatabasesAsync();
			}

			return NoContent();
		}
	}
}
