namespace TestOkur.Notification.Infrastructure.Clients
{
	using System.Threading.Tasks;

	public interface IOAuthClient
	{
		Task<string> GetToken();
	}
}