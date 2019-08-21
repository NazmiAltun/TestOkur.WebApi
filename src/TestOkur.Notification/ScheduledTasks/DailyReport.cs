namespace TestOkur.Notification.ScheduledTasks
{
	using System.Threading.Tasks;
	using TestOkur.Notification.Infrastructure;
	using TestOkur.Notification.Models;

	internal class DailyReport : IDailyReport
	{
		private readonly INotificationFacade _notificationFacade;

		public DailyReport(INotificationFacade notificationFacade)
		{
			_notificationFacade = notificationFacade;
		}

		public async Task SendAsync()
		{
			await _notificationFacade.SendEmailToSystemAdminsAsync(
				new DailyReportModel(),
				Template.DailyReportEmailAdmin);
		}
	}
}
