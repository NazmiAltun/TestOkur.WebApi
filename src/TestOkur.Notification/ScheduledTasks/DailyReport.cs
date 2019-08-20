namespace TestOkur.Notification.ScheduledTasks
{
	using System.Threading.Tasks;
	using TestOkur.Notification.Models;

	internal class DailyReport : IDailyReport
	{
		private readonly NotificationManager _notificationManager;

		public DailyReport(NotificationManager notificationManager)
		{
			_notificationManager = notificationManager;
		}

		public async Task SendAsync()
		{
			await _notificationManager.SendEmailToSystemAdminsAsync(
				new DailyReportModel(),
				Template.DailyReportEmailAdmin);
		}
	}
}
