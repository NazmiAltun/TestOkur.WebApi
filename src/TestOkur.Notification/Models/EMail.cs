namespace TestOkur.Notification.Models
{
    using System;

    public class EMail
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime SentOnUtc { get; set; } = DateTime.UtcNow;

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Receiver { get; set; }
    }
}
