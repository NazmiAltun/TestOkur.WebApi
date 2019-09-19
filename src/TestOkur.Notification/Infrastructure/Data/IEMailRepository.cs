namespace TestOkur.Notification.Infrastructure.Data
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface IEMailRepository
    {
        Task AddAsync(EMail email);
    }
}