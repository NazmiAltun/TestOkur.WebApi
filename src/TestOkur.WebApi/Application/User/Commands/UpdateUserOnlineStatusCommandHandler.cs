namespace TestOkur.WebApi.Application.User.Commands
{
    using CacheManager.Core;
    using Paramore.Brighter;
    using System;
    using System.Collections.Concurrent;

    public class UpdateUserOnlineStatusCommandHandler : RequestHandler<UpdateUserOnlineStatusCommand>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<ConcurrentDictionary<string, DateTime>> _userCacheManager;

        public UpdateUserOnlineStatusCommandHandler(ICacheManager<ConcurrentDictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override UpdateUserOnlineStatusCommand Handle(UpdateUserOnlineStatusCommand command)
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new ConcurrentDictionary<string, DateTime>();
            usersDictionary.AddOrUpdate(command.Email, DateTime.UtcNow, (k, v) => DateTime.UtcNow);
            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            return command;
        }
    }
}
