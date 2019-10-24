namespace TestOkur.Notification.Models
{
    using System;
    using TestOkur.Contracts.Sms;
    using TestOkur.Notification.Extensions;

    public class Sms
    {
        public Sms()
        {
            CreatedOnDateTimeUtc = DateTime.UtcNow;
        }

        public Sms(ISendSmsRequestReceived @event, ISmsMessage smsMessage)
        {
            Subject = smsMessage.Subject;
            Body = smsMessage.Body.ToSmsFriendly();
            Phone = smsMessage.Receiver;
            Credit = smsMessage.Credit;
            Status = SmsStatus.Pending;
            Id = smsMessage.Id;
            StudentOpticalFormId = smsMessage.StudentOpticalFormId;
            ExamId = smsMessage.ExamId;
            UserId = @event.UserId;
            CreatedOnDateTimeUtc = @event.CreatedOnUTC;
            UserSubjectId = @event.UserSubjectId;
        }

        public Guid Id { get; set; }

        public int UserId { get; set; }

        public string UserSubjectId { get; set; }

        public string Phone { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public string ServiceRequest { get; set; }

        public string ServiceResponse { get; set; }

        public string UserFriendlyErrorMessage { get; set; }

        public string Error { get; set; }

        public int Credit { get; set; }

        public string StudentOpticalFormId { get; set; }

        public int ExamId { get; set; }

        public DateTime CreatedOnDateTimeUtc { get; set; }

        public DateTime RequestDateTimeUtc { get; set; }

        public DateTime ResponseDateTimeUtc { get; set; }

        public SmsStatus Status { get; set; }
    }
}
