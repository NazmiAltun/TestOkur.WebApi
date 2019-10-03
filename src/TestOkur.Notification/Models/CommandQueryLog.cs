namespace TestOkur.Notification.Models
{
    using System;
    using TestOkur.Contracts.App;

    public class CommandQueryLog
    {
        public CommandQueryLog(ICommandQueryLogEvent contextMessage)
        {
            Message = contextMessage.Message;
            Type = contextMessage.Type;
            Id = contextMessage.Id;
            CreatedOnUTC = contextMessage.CreatedOnUTC;
        }

        public string Message { get; set; }

        public string Type { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedOnUTC { get; set; }
    }
}
