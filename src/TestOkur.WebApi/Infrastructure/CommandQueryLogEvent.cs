namespace TestOkur.WebApi.Infrastructure
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.App;

    public class CommandQueryLogEvent : IntegrationEvent, ICommandQueryLogEvent
    {
        public CommandQueryLogEvent(string message, string type)
        {
            Message = message;
            Type = type;
        }

        public string Type { get; }

        public string Message { get; }
    }
}