namespace TestOkur.Notification.Infrastructure
{
	using System.Threading.Tasks;
	using TestOkur.Notification.Models;

	public interface IWebApiClient
	{
		Task<AppSettingReadModel> GetAppSettingAsync(string name);

		Task DeductSmsCreditsAsync(int userId, string smsBody);
	}
}