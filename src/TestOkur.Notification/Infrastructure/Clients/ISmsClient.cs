namespace TestOkur.Notification.Infrastructure.Clients
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Dtos;

    public interface ISmsClient
    {
        Task<string> SendAsync(Sms sms);
    }
}