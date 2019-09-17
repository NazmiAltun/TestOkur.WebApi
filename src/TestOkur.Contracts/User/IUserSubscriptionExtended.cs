namespace TestOkur.Contracts.User
{
    using System;

    public interface IUserSubscriptionExtended : IIntegrationEvent
	{
		string FirstName { get; }

		string LastName { get; }

		string Email { get; }

		string Phone { get; }

		DateTime ExpiryDate { get; }
	}
}
