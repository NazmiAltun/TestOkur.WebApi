namespace TestOkur.Contracts.Account
{
	using System;

	public interface IAccountExtended : IIntegrationEvent
	{
		string FirstName { get; }

		string LastName { get; }

		string Email { get; }

		string Password { get; }

		string Phone { get; }

		DateTime ExpiryDate { get; }
	}
}
