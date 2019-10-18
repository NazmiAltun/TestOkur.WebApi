namespace TestOkur.WebApi.Application.User.Queries
{
    using CacheManager.Core;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class GetOnlineUsersQueryHandler : QueryHandler<GetOnlineUsersQuery, IReadOnlyCollection<string>>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<Dictionary<string, DateTime>> _userCacheManager;

        public GetOnlineUsersQueryHandler(ICacheManager<Dictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override IReadOnlyCollection<string> Execute(GetOnlineUsersQuery query)
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new Dictionary<string, DateTime>();

            foreach (var key in usersDictionary.Keys)
            {
                if (DateTime.UtcNow.Subtract(usersDictionary[key]).TotalMinutes > 1)
                {
                    usersDictionary.Remove(key);
                }
            }

            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            return usersDictionary.Keys.ToList();
        }
    }
}
