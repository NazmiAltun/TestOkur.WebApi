namespace TestOkur.Notification.Infrastructure
{
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using TestOkur.Notification.Models;

    public class EmailBuilder<TModel>
    {
        private readonly ITemplateService _templateEngine;

        private Template _template;
        private TModel _model;
        private string[] _receivers;

        public EmailBuilder(ITemplateService templateEngine)
        {
            _templateEngine = templateEngine;
        }

        public EmailBuilder<TModel> WithTemplate(Template template)
        {
            _template = template;

            return this;
        }

        public EmailBuilder<TModel> WithModel(TModel model)
        {
            _model = model;

            return this;
        }

        public EmailBuilder<TModel> WithReceivers(string receivers)
        {
            _receivers = receivers.Split(';');

            return this;
        }

        public async Task<MailMessage> Build()
        {
            var mail = new MailMessage
            {
                Subject = _template.Subject,
                Body = await _templateEngine.RenderTemplateAsync(Path.Join("Email", _template.BodyPath), _model),
            };

            foreach (var receiver in _receivers)
            {
                mail.To.Add(receiver);
            }

            return mail;
        }
    }
}
