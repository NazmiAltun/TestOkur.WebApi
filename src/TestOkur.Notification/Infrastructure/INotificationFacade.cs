namespace TestOkur.Notification.Infrastructure
{
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public interface INotificationFacade
    {
        Task SendSmsAsync<TModel>(TModel model, Template template, string receiver);

        Task SendEmailToSystemAdminsAsync<TModel>(TModel model, Template template);

        Task SendEmailToProductOwnersAsync<TModel>(TModel model, Template template);

        Task SendEmailAsync<TModel>(TModel model, Template template, string receiver);
    }
}