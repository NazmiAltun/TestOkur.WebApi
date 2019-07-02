namespace TestOkur.WebApi.Application.Sms.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using TestOkur.Infrastructure.Cqrs;

	[DataContract]
	public class SendSmsCommand : CommandBase
	{
		public SendSmsCommand(
			Guid id,
			IEnumerable<SmsMessage> messages)
		 : base(id)
		{
			Messages = messages;
		}

		[DataMember]
		public IEnumerable<SmsMessage> Messages { get; private set; }
	}
}
