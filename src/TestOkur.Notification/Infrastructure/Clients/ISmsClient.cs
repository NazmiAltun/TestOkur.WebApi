namespace TestOkur.Notification.Infrastructure
{
	using System.Threading.Tasks;
	using TestOkur.Notification.Models;

	public interface ISmsClient
	{
		Task<string> SendAsync(Sms sms);
	}
}