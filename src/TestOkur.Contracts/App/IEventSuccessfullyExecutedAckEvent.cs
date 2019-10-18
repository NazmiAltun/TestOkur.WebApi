namespace TestOkur.Contracts.App
{
    using System;

    public interface IEventSuccessfullyExecutedAckEvent : IIntegrationEvent
    {
        Guid ExecutedEventId { get; }

        DateTime ExecutedOnUtc { get; }
    }
}
