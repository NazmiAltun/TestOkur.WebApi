namespace TestOkur.WebApi.Application.Sms.Commands
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.Sms;

    public class SmsCreditAdded : IntegrationEvent, ISmsCreditAdded
	{
		public SmsCreditAdded(int amount, int totalSmsCredits, string firstName, string lastName, string email, string phone)
		{
			Amount = amount;
			TotalSmsCredits = totalSmsCredits;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Phone = phone;
		}

		public int Amount { get; set; }

		public int TotalSmsCredits { get; }

		public string FirstName { get; }

		public string LastName { get; }

		public string Email { get; set; }

		public string Phone { get; set; }
	}
}
