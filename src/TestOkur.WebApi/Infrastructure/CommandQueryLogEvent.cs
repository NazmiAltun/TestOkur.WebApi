namespace TestOkur.WebApi.Infrastructure
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.App;

    public class CommandQueryLogEvent : IntegrationEvent, ICommandQueryLogEvent
    {
        public CommandQueryLogEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}