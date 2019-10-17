namespace TestOkur.WebApi.Application.User.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Paramore.Darker;

    //TODO:Fix
    public sealed class GetOnlineUsersQueryHandler : QueryHandlerAsync<GetOnlineUsersQuery, IReadOnlyCollection<string>>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<Dictionary<string, DateTime>> _userCacheManager;

        public GetOnlineUsersQueryHandler(ICacheManager<Dictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override async Task<IReadOnlyCollection<string>> ExecuteAsync(GetOnlineUsersQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new Dictionary<string, DateTime>();

            foreach (var key in usersDictionary.Keys)
            {
                if (usersDictionary[key].Subtract(DateTime.UtcNow).TotalMinutes > 1)
                {
                    usersDictionary.Remove(key);
                }
            }

            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            await Task.Delay(1, cancellationToken);
            return usersDictionary.Keys.ToList();
        }
    }
}
