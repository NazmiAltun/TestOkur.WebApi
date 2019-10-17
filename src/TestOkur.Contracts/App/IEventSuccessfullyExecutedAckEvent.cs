using System;

namespace TestOkur.Contracts.App
{
    public interface IEventSuccessfullyExecutedAckEvent : IIntegrationEvent
    {
        Guid ExecutedEventId { get; }

        DateTime ExecutedOnUtc { get; }
    }
}
