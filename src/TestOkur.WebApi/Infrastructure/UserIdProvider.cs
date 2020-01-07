namespace TestOkur.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Dapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Infrastructure.Mvc.Extensions;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Configuration;

    public class UserIdProvider : IUserIdProvider
    {
        private const string CacheKey = "UserIdMap";

        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(4);
        private static readonly ReaderWriterLockSlim ReaderWriterLockSlim = new ReaderWriterLockSlim();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ILogger<UserIdProvider> _logger;
        private readonly string _connectionString;

        public UserIdProvider(
            ApplicationConfiguration configurationOptions,
            IHttpContextAccessor httpContextAccessor,
            ICacheManager<object> cacheManager,
            ILogger<UserIdProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor ??
                                   throw new ArgumentNullException(nameof(httpContextAccessor));
            _cacheManager = cacheManager;
            _logger = logger;
            _connectionString = configurationOptions.Postgres;
        }

        public async Task<int> GetAsync()
        {
            var subjectId = _httpContextAccessor.GetUserId();

            if (subjectId == null)
            {
                _logger.LogInformation("SubjectId not found");
                return default;
            }

            var idDictionary = (Dictionary<string, int>)_cacheManager.Get(CacheKey);
            ReaderWriterLockSlim.EnterUpgradeableReadLock();

            if (idDictionary == null)
            {
                ReaderWriterLockSlim.EnterWriteLock();
                idDictionary = await ReadIdsFromDbAsync();
                StoreToCache(idDictionary);
                ReaderWriterLockSlim.ExitWriteLock();
            }

            ReaderWriterLockSlim.ExitUpgradeableReadLock();
            _logger.LogInformation($"idDictionary.ContainsKey(${subjectId}) : {idDictionary.ContainsKey(subjectId)}");

            return idDictionary.TryGetValue(subjectId, out var id) ? id : 0;
        }

        public int Get() => GetAsync().GetAwaiter().GetResult();

        private async Task<Dictionary<string, int>> ReadIdsFromDbAsync()
        {
            const string sql = "SELECT id,subject_id FROM users";

            await using var connection = new NpgsqlConnection(_connectionString);
            return (await connection.QueryAsync<UserReadModel>(sql))
                .ToDictionary(u => u.SubjectId, u => u.Id);
        }

        private void StoreToCache(Dictionary<string, int> idDictionary)
        {
            _cacheManager.Add(new CacheItem<object>(
                CacheKey,
                idDictionary,
                ExpirationMode.Absolute,
                CacheDuration));
        }
    }
}
