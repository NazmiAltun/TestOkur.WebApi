namespace TestOkur.WebApi.Application.User.Events
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.User;

	public class UserActivated : IntegrationEvent, IUserActivated
	{
		public UserActivated(string phone, string email, string lastName, string firstName)
		{
			Phone = phone;
			Email = email;
			LastName = lastName;
			FirstName = firstName;
		}

		public string FirstName { get; }

		public string LastName { get; }

		public string Email { get; }

		public string Phone { get; }
	}
}
