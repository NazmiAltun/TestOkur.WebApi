namespace TestOkur.WebApi.Application.Sms
{
    using System;
    using TestOkur.Contracts.Sms;

    public class SmsMessage : ISmsMessage
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
        {
            Receiver = receiver;
            Subject = subject;
            Body = body;
            Credit = credit;
            Id = id;
        }

        public string Receiver { get; }

        public int Credit { get; }

        public string StudentOpticalFormId { get; }

        public int ExamId { get; }

        public Guid Id { get; }

        public string Subject { get; }

        public string Body { get; }
    }
}
