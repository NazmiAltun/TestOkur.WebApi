namespace TestOkur.Notification.Models
{
    using System;

    public class UserSmsModel
    {
        public UserSmsModel(Sms sms)
        {
            Phone = sms.Phone;
            Subject = sms.Subject;
            Body = sms.Body;
            Credit = sms.Credit;
            RequestDateTimeUtc = sms.RequestDateTimeUtc;
            ResponseDateTimeUtc = sms.ResponseDateTimeUtc;
            Status = sms.Status;
        }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int Credit { get; set; }

        public DateTime RequestDateTimeUtc { get; set; }

        public DateTime ResponseDateTimeUtc { get; set; }

        public SmsStatus Status { get; set; }
    }
}
