namespace TestOkur.Notification.Extensions
{
    using System.Linq;
    using System.Net.Mail;
    using TestOkur.Notification.Models;

    public static class MailMessageExtensions
    {
        public static EMail ToEMail(this MailMessage mailMessage)
        {
            return new EMail
            {
                Subject = mailMessage.Subject,
                Body = mailMessage.Body,
                Receiver = string.Join(';', mailMessage.To.Select(x => x.Address)),
            };
        }
    }
}
