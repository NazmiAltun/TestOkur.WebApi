namespace TestOkur.WebApi.Application.Sms
{
    using System;
    using TestOkur.Contracts.Sms;

    public class SmsMessage : SmsMessageModel, ISmsMessage
    {
        public SmsMessage(SmsMessageModel model, int credit)
        : this(model.Receiver, model.Subject, model.Body, credit)
        {
        }

        public SmsMessage(string receiver, string subject, string body, int credit)
         : this(receiver, subject, body, credit, Guid.NewGuid())
        {
        }

        public SmsMessage(string receiver, string subject, string body, int credit, Guid id)
         : base(receiver, subject, body)
        {
            Credit = credit;
            Id = id;
        }

        public int Credit { get; }

        public Guid Id { get; }
    }
}
