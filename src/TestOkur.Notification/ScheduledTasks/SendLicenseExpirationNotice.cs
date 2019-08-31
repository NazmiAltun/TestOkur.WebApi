namespace TestOkur.Notification.ScheduledTasks
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Logging;
	using TestOkur.Notification.Configuration;
	using TestOkur.Notification.Extensions;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Infrastructure.Clients;
	using TestOkur.Notification.Models;

	internal class SendLicenseExpirationNotice : ISendLicenseExpirationNotice
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IOAuthClient _oAuthClient;
		private readonly IWebApiClient _webApiClient;
		private readonly INotificationFacade _notificationFacade;
		private readonly ApplicationConfiguration _applicationConfiguration;
		private readonly ILogger<SendLicenseExpirationNotice> _logger;

		public SendLicenseExpirationNotice(
			INotificationFacade notificationFacade,
			IOAuthClient oAuthClient,
			IWebApiClient webApiClient,
			ApplicationConfiguration applicationConfiguration,
			IHostingEnvironment hostingEnvironment,
			ILogger<SendLicenseExpirationNotice> logger)
		{
			_notificationFacade = notificationFacade;
			_oAuthClient = oAuthClient;
			_webApiClient = webApiClient;
			_applicationConfiguration = applicationConfiguration;
			_hostingEnvironment = hostingEnvironment;
			_logger = logger;
		}

		public async Task NotifyUsersAsync()
		{
			foreach (var user in await GetUsersAsync())
			{
				if (_hostingEnvironment.IsProd())
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
				else
				{
					_logger.LogWarning($"License Expiration Notification for {user.Email}");
				}
			}
		}

		private async Task<IEnumerable<UserModel>> GetUsersAsync()
		{
			var identityUsers = await _oAuthClient.GetUsersAsync();
			var apiUsers = await _webApiClient.GetUsersAsync();

			return (from user in identityUsers
					where user.Active && user.ExpiryDateUtc != null && Math.Round(user.ExpiryDateUtc.Value.Subtract(DateTime.UtcNow).TotalDays) == _applicationConfiguration.RemainderDays
					select apiUsers.First(u => u.SubjectId == user.Id))
				.ToList();
		}
	}
}