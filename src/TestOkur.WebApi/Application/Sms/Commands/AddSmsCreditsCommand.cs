namespace TestOkur.WebApi.Application.Sms.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Paramore.Brighter;
    using TestOkur.Infrastructure.Cqrs;

    [DataContract]
    public class AddSmsCreditsCommand : Command, IClearCache
	{
		public AddSmsCreditsCommand(Guid commandId, int userId, int amount)
			: base(commandId)
		{
			UserId = userId;
			Amount = amount;
		}

		[DataMember]
		public int UserId { get; private set; }

		[DataMember]
		public int Amount { get; private set; }

		public IEnumerable<string> CacheKeys => new[] { "Users" };
	}
}
