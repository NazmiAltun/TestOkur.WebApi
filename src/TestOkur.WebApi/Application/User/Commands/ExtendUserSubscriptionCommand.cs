namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class ExtendUserSubscriptionCommand : CommandBase, IClearCache
    {
        public ExtendUserSubscriptionCommand(string email, DateTime currentExpiryDateTimeUtc)
        {
            Email = email;
            CurrentExpiryDateTimeUtc = currentExpiryDateTimeUtc;
        }

        public ExtendUserSubscriptionCommand()
        {
        }

        public string Email { get; set; }

        public DateTime CurrentExpiryDateTimeUtc { get; set; }

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };
    }
}
