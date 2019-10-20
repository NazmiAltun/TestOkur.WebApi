namespace TestOkur.WebApi.Application.User.Queries
{
    using CacheManager.Core;
    using Paramore.Darker;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed class GetOnlineUsersQueryHandler : QueryHandler<GetOnlineUsersQuery, IEnumerable<string>>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<ConcurrentDictionary<string, DateTime>> _userCacheManager;

        public GetOnlineUsersQueryHandler(ICacheManager<ConcurrentDictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override IEnumerable<string> Execute(GetOnlineUsersQuery query)
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new ConcurrentDictionary<string, DateTime>();

            foreach (var key in usersDictionary.Keys)
            {
                if (DateTime.UtcNow.Subtract(usersDictionary[key]).TotalMinutes > 1)
                {
                    usersDictionary.Remove(key, out _);
                }
            }

            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            return usersDictionary.Keys;
        }
    }
}
