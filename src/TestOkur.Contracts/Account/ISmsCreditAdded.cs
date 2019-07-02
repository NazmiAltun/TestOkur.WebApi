namespace TestOkur.Contracts.Account
{
	public interface ISmsCreditAdded : IIntegrationEvent
	{
		string FirstName { get; }

		string LastName { get; }

		int AddedSmsCreditAmount { get; }

		int TotalSmsCredit { get; }

		string Email { get; }

		string Phone { get; }
	}
}
