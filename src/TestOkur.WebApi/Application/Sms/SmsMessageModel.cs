namespace TestOkur.WebApi.Application.Sms
{
    public class SmsMessageModel
    {
        public SmsMessageModel(string receiver, string subject, string body)
        {
            Subject = subject;
            Receiver = receiver;
            Body = body;
        }

        public SmsMessageModel()
        {
        }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Receiver { get; set; }

        public string StudentOpticalFormId { get; set; }

        public int ExamId { get; set; }
    }
}
