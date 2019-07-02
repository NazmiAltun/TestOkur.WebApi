namespace TestOkur.Contracts
{
	using System;

	public interface IIntegrationEvent
    {
        Guid Id { get; }

        DateTime CreatedOnUTC { get; }
    }
}
