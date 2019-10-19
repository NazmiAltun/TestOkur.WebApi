namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Dtos;

    public interface IStatsRepository
    {
        Task<NotificationStatisticsDto> GetStatisticsAsync();
    }
}