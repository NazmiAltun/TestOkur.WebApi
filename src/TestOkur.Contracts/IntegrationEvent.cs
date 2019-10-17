namespace TestOkur.Contracts
{
    using System;

    public abstract class IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedOnUTC = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime CreatedOnUTC { get; }
    }
}
