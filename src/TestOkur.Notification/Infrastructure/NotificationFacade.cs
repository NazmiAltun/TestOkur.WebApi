namespace TestOkur.Notification.Infrastructure
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using TestOkur.Common;
    using TestOkur.Notification.Extensions;
    using TestOkur.Notification.Infrastructure.Clients;
    using TestOkur.Notification.Infrastructure.Data;
    using TestOkur.Notification.Models;

    internal class NotificationFacade : INotificationFacade
    {
        private readonly ISmsClient _smsClient;
        private readonly IEmailClient _emailClient;
        private readonly IWebApiClient _webApiClient;
        private readonly ITemplateService _templateEngine;
        private readonly ISmsRepository _smsRepository;

        public NotificationFacade(
            IEmailClient emailClient,
            ITemplateService templateEngine,
            IWebApiClient webApiClient,
            ISmsClient smsClient,
            ISmsRepository smsRepository)
        {
            _emailClient = emailClient ?? throw new ArgumentNullException(nameof(emailClient));
            _templateEngine = templateEngine ?? throw new ArgumentNullException(nameof(templateEngine));
            _webApiClient = webApiClient ?? throw new ArgumentNullException(nameof(webApiClient));
            _smsClient = smsClient ?? throw new ArgumentNullException(nameof(smsClient));
            _smsRepository = smsRepository ?? throw new ArgumentNullException(nameof(smsRepository));
        }

        public async Task SendSmsAsync<TModel>(TModel model, Template template, string receiver)
        {
            var body = await _templateEngine.RenderTemplateAsync(
                Path.Join("Sms", template.BodyPath), model);
            var sms = new Sms()
            {
                Id = Guid.NewGuid(),
                Body = body.ToSmsFriendly(),
                Phone = receiver,
                Subject = template.Subject,
            };
            await _smsRepository.AddAsync(sms);
            await _smsClient.SendAsync(sms);
        }

        public async Task SendEmailToSystemAdminsAsync<TModel>(TModel model, Template template)
        {
            await SendEmailAsync(
                model,
                template,
                (await _webApiClient.GetAppSettingAsync(AppSettings.SystemAdminEmails)).Value);
        }

        public async Task SendEmailToAdminsAsync<TModel>(TModel model, Template template)
        {
            await SendEmailAsync(
                model,
                template,
                (await _webApiClient.GetAppSettingAsync(AppSettings.AdminEmails)).Value);
        }

        public async Task SendEmailAsync<TModel>(TModel model, Template template, string receiver)
        {
            var email = await new EmailBuilder<TModel>(_templateEngine)
                .WithTemplate(template)
                .WithModel(model)
                .WithReceivers(receiver)
                .Build();

            await _emailClient.SendAsync(email);
        }
    }
}
