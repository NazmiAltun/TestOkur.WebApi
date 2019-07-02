namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using Paramore.Brighter;
	using TestOkur.Infrastructure.Cqrs;

	[DataContract]
	public class DeductSmsCreditsCommand : Command, IClearCache
	{
        public DeductSmsCreditsCommand(Guid commandId, int userId, string smsBody)
            : base(commandId)
        {
	        UserId = userId;
	        SmsBody = smsBody;
        }

        public int UserId { get; private set; }

        public string SmsBody { get; private set; }

        public IEnumerable<string> CacheKeys => new[] { "Users" };
	}
}