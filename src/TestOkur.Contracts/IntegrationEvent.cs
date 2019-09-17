namespace TestOkur.Contracts
{
    using System;

    public class IntegrationEvent : IIntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedOnUTC = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime CreatedOnUTC { get; }
    }
}
