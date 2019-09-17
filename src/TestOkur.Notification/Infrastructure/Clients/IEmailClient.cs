namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface IEmailClient
	{
		Task SendAsync(MailMessage mailMessage);
	}
}