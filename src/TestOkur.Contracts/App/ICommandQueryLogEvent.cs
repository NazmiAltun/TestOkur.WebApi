namespace TestOkur.Contracts.App
{
    public interface ICommandQueryLogEvent : IIntegrationEvent
    {
        string Message { get; }
    }
}
