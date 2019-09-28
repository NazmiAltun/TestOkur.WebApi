namespace TestOkur.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Dapper;
    using IdentityModel;
    using Microsoft.AspNetCore.Http;
    using Npgsql;
    using TestOkur.Infrastructure;
    using TestOkur.WebApi.Application.User.Queries;
    using TestOkur.WebApi.Configuration;

    public class UserIdProvider : IUserIdProvider
    {
        private const string CacheKey = "UserIdMap";

        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
        private static ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheManager<Dictionary<string, int>> _cacheManager;
        private readonly string _connectionString;

        public UserIdProvider(
            ApplicationConfiguration configurationOptions,
            IHttpContextAccessor httpContextAccessor,
            ICacheManager<Dictionary<string, int>> cacheManager)
        {
            _httpContextAccessor = httpContextAccessor ??
                                   throw new ArgumentNullException(nameof(httpContextAccessor));
            _cacheManager = cacheManager;
            _connectionString = configurationOptions.Postgres;
        }

        public async Task<int> GetAsync()
        {
            var subjectId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(JwtClaimTypes.Subject)?.Value;

            if (subjectId == null)
            {
                return default;
            }

            CacheLock.EnterUpgradeableReadLock();
            var idDictionary = _cacheManager.Get(CacheKey);

            if (idDictionary == null)
            {
                CacheLock.EnterWriteLock();
                try
                {
                    idDictionary = await ReadIdsFromDbAsync();
                    StoreToCache(idDictionary);
                }
                finally
                {
                    CacheLock.ExitWriteLock();
                }
            }

            var foundId = idDictionary.TryGetValue(subjectId, out var id) ? id : 0;
            CacheLock.ExitUpgradeableReadLock();

            return foundId;
        }

        private async Task<Dictionary<string, int>> ReadIdsFromDbAsync()
        {
            const string sql = "SELECT id,subject_id FROM users";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<UserReadModel>(sql))
                    .ToDictionary(u => u.SubjectId, u => u.Id);
            }
        }

        private void StoreToCache(Dictionary<string, int> idDictionary)
        {
            _cacheManager.Add(new CacheItem<Dictionary<string, int>>(
                CacheKey,
                idDictionary,
                ExpirationMode.Absolute,
                CacheDuration));
        }
    }
}
