namespace TestOkur.WebApi.Application.Sms.Commands
{
    using Paramore.Brighter;
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class AddSmsCreditsCommand : Command, IClearCache
    {
        public AddSmsCreditsCommand(Guid commandId, int userId, int amount, bool gift)
            : base(commandId)
        {
            UserId = userId;
            Amount = amount;
            Gift = gift;
        }

        public AddSmsCreditsCommand()
         : base(Guid.NewGuid())
        {
        }

        public int UserId { get; set; }

        public int Amount { get; set; }

        public bool Gift { get; set; }

        public IEnumerable<string> CacheKeys => new[] { "Users" };
    }
}
