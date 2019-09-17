namespace TestOkur.WebApi.Application.Sms
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SmsMessageModel
    {
        public SmsMessageModel(string receiver, string subject, string body)
        {
            Subject = subject;
            Receiver = receiver;
            Body = body;
        }

        [DataMember]
        public string Subject { get; private set; }

        [DataMember]
        public string Body { get; private set; }

        [DataMember]
        public string Receiver { get; private set; }

        [DataMember]
        public string StudentOpticalFormId { get; private set; }

        [DataMember]
        public int ExamId { get; private set; }
	}
}
