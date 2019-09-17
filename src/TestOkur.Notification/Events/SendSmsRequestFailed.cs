namespace TestOkur.Notification.Events
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Sms;

    internal class SendSmsRequestFailed : IntegrationEvent, ISendSmsRequestFailed
    {
        public SendSmsRequestFailed(
            int userId,
            string receiver,
            string smsBody,
            string errorMessage,
            string userFriendlyMessage,
            string userEmail)
        {
            UserId = userId;
            ErrorMessage = errorMessage;
            UserFriendlyMessage = userFriendlyMessage;
            UserEmail = userEmail;
            Receiver = receiver;
            SmsBody = smsBody;
        }

        public int UserId { get; }

        public string Receiver { get; }

        public string SmsBody { get; }

        public string ErrorMessage { get; }

        public string UserFriendlyMessage { get; }

        public string UserEmail { get; }
    }
}