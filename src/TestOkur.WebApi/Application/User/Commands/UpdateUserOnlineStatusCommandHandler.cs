namespace TestOkur.WebApi.Application.User.Commands
{
    using CacheManager.Core;
    using Paramore.Brighter;
    using System;
    using System.Collections.Generic;

    public class UpdateUserOnlineStatusCommandHandler : RequestHandler<UpdateUserOnlineStatusCommand>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<Dictionary<string, DateTime>> _userCacheManager;

        public UpdateUserOnlineStatusCommandHandler(ICacheManager<Dictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override UpdateUserOnlineStatusCommand Handle(UpdateUserOnlineStatusCommand command)
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new Dictionary<string, DateTime>();
            if (!usersDictionary.TryAdd(command.Email, DateTime.UtcNow))
            {
                usersDictionary[command.Email] = DateTime.UtcNow;
            }

            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            return command;
        }
    }
}
