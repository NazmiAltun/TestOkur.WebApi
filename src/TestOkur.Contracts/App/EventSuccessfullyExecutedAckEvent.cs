namespace TestOkur.Contracts.App
{
    using System;

    public class EventSuccessfullyExecutedAckEvent : IntegrationEvent, IEventSuccessfullyExecutedAckEvent
    {
        public EventSuccessfullyExecutedAckEvent(Guid executedEventId, DateTime executedOnUtc)
        {
            ExecutedEventId = executedEventId;
            ExecutedOnUtc = executedOnUtc;
        }

        public Guid ExecutedEventId { get; set; }

        public DateTime ExecutedOnUtc { get; set; }
    }
}