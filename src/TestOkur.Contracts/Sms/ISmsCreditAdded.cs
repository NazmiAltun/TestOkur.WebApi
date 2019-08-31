namespace TestOkur.Contracts.Sms
{
	public interface ISmsCreditAdded : IIntegrationEvent
	{
		int Amount { get; set; }

		int TotalSmsCredits { get; }

		string FirstName { get; }

		string LastName { get; }

		string Email { get; set; }

		string Phone { get; set; }
	}
}
