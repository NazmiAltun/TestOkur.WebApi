namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Paramore.Brighter;

    //TODO:Convert to RequestHandler
    public class UpdateUserOnlineStatusCommandHandler
        : RequestHandlerAsync<UpdateUserOnlineStatusCommand>
    {
        private const string CacheKey = "OnlineUsers";

        private readonly ICacheManager<Dictionary<string, DateTime>> _userCacheManager;

        public UpdateUserOnlineStatusCommandHandler(ICacheManager<Dictionary<string, DateTime>> userCacheManager)
        {
            _userCacheManager = userCacheManager;
        }

        public override async Task<UpdateUserOnlineStatusCommand> HandleAsync(
            UpdateUserOnlineStatusCommand command,
            CancellationToken cancellationToken = default)
        {
            var usersDictionary = _userCacheManager.Get(CacheKey) ?? new Dictionary<string, DateTime>();
            if (!usersDictionary.TryAdd(command.Email, DateTime.UtcNow))
            {
                usersDictionary[command.Email] = DateTime.UtcNow;
            }

            _userCacheManager.AddOrUpdate(CacheKey, usersDictionary, (d) => usersDictionary);

            await Task.Delay(1, cancellationToken);
            return command;
        }
    }
}
