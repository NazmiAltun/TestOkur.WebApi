namespace TestOkur.WebApi.Application.Sms
{
	using System;
	using System.Runtime.Serialization;
	using TestOkur.Contracts.Sms;

	[DataContract]
	public class SmsMessage : ISmsMessage
    {
        public SmsMessage(string receiver, string subject, string body)
		 : this()
        {
            Subject = subject;
            Receiver = receiver;
            Body = body;
        }

        public SmsMessage()
        {
			Id = Guid.NewGuid();
        }

        [DataMember]
        public string Subject { get; private set; }

        [DataMember]
        public string Body { get; private set; }

        [DataMember]
        public string Receiver { get; private set; }

        [DataMember]
        public int Credit { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}
