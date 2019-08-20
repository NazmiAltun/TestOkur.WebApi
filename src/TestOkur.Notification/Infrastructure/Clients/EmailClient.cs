namespace TestOkur.Notification.Infrastructure.Clients
{
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;
	using TestOkur.Notification.Configuration;

	public class EmailClient : IEmailClient
	{
		private readonly SmtpConfiguration _smtpConfiguration;

		public EmailClient(SmtpConfiguration smtpConfiguration)
		{
			_smtpConfiguration = smtpConfiguration;
		}

		public async Task SendAsync(MailMessage mailMessage)
		{
			using (var client = new SmtpClient(_smtpConfiguration.Host, _smtpConfiguration.Port))
			{
				client.EnableSsl = _smtpConfiguration.EnableSsl;
				client.UseDefaultCredentials = _smtpConfiguration.UseDefaultCredentials;
				client.Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password);
				mailMessage.From = new MailAddress(_smtpConfiguration.Username, _smtpConfiguration.FromName);
				mailMessage.IsBodyHtml = true;
				mailMessage.Priority = MailPriority.Normal;
				mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

				await client.SendMailAsync(mailMessage);
			}
		}
	}
}
