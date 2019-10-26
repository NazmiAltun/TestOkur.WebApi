namespace TestOkur.WebApi.Application.Sms
{
    using System.Collections.Generic;
    using System.Linq;
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
            CreditAmount = smsMessages.Sum(m => m.Credit);
            UserSubjectId = userSubjectId;
        }

        public int UserId { get; }

        public string UserSubjectId { get; }

        public string UserEmail { get; }

        public IEnumerable<ISmsMessage> SmsMessages { get; }

        public int CreditAmount { get; }
    }
}
