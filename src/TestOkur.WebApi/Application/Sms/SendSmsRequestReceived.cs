namespace TestOkur.WebApi.Application.Sms
{
    using System.Collections.Generic;
    using TestOkur.Contracts;
    using TestOkur.Contracts.Sms;

    public class SendSmsRequestReceived : IntegrationEvent, ISendSmsRequestReceived
    {
        public SendSmsRequestReceived(
            int userId,
            string userSubjectId,
            IEnumerable<ISmsMessage> smsMessages,
            string userEmail)
        {
            UserId = userId;
            SmsMessages = smsMessages;
            UserEmail = userEmail;
            UserSubjectId = userSubjectId;
        }

        public int UserId { get; }

        public string UserSubjectId { get; }

        public string UserEmail { get; }

        public IEnumerable<ISmsMessage> SmsMessages { get; }
    }
}
