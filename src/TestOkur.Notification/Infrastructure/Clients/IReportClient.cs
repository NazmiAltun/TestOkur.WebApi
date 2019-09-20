namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IReportClient
    {
        Task<ReportStatisticsModel> GetStatisticsAsync();
    }
}