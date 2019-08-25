namespace TestOkur.Notification.ScheduledTasks
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TestOkur.Notification.Configuration;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Infrastructure.Clients;
	using TestOkur.Notification.Models;

	internal class SendLicenseExpirationNotice : ISendLicenseExpirationNotice
	{
		private readonly IOAuthClient _oAuthClient;
		private readonly IWebApiClient _webApiClient;
		private readonly INotificationFacade _notificationFacade;
		private readonly ApplicationConfiguration _applicationConfiguration;
		private readonly ILogger<SendLicenseExpirationNotice> _logger;

		public SendLicenseExpirationNotice(
			INotificationFacade notificationFacade,
			IOAuthClient oAuthClient,
			IWebApiClient webApiClient,
			ApplicationConfiguration applicationConfiguration)
		{
			_notificationFacade = notificationFacade;
			_oAuthClient = oAuthClient;
			_webApiClient = webApiClient;
			_applicationConfiguration = applicationConfiguration;
		}

		public async Task NotifyUsersAsync()
		{
			foreach (var user in await GetUsersAsync())
			{
				await _notificationFacade.SendEmailAsync(
					user,
					Template.LicenseExpirationNoticeEmailUser,
					user.Email);
				await _notificationFacade.SendSmsAsync(
					user,
					Template.LicenseExpirationNoticeSms,
					user.Phone);
			}
		}

		private async Task<IEnumerable<UserModel>> GetUsersAsync()
		{
			var identityUsers = await _oAuthClient.GetUsersAsync();
			var apiUsers = await _webApiClient.GetUsersAsync();

			return (from user in identityUsers
                where user.Active && user.ExpiryDateUtc != null && DateTime.UtcNow.Subtract(user.ExpiryDateUtc.Value).Days == _applicationConfiguration.RemainderDays
				select apiUsers.First(u => u.SubjectId == user.Id))
				.ToList();
		}
	}
}