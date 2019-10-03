namespace TestOkur.Contracts.App
{
    public interface ICommandQueryLogEvent : IIntegrationEvent
    {
        string Type { get; }

        string Message { get; }
    }
}
