namespace TestOkur.Notification.Infrastructure
{
	using System.Net.Mail;
	using System.Threading.Tasks;

	public interface IEmailClient
	{
		Task SendAsync(MailMessage mailMessage);
	}
}