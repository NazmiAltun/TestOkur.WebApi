namespace TestOkur.Notification.ScheduledTasks
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using TestOkur.Notification.Models;

	internal class SendLicenseExpirationNotice : ISendLicenseExpirationNotice
	{
		private readonly NotificationManager _notificationManager;

		public SendLicenseExpirationNotice(NotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		public async Task NotifyUsersAsync()
		{
			foreach (var user in GetUsers())
			{
				await _notificationManager.SendEmailAsync(
					user,
					Template.LicenseExpirationNoticeEmailUser,
					user.Email);
				await _notificationManager.SendSmsAsync(
					user,
					Template.LicenseExpirationNoticeSms,
					user.Phone);
			}
		}

		private List<UserModel> GetUsers()
		{
			return new List<UserModel>
			{
				new UserModel()
				{
					Email = "nazmialtun@windowslive.com",
					Phone = "5544205163",
				},
				new UserModel()
				{
					Email = "necatiyalcin@gmail.com",
					Phone = "5074011191",
				},
			};
		}
	}
}