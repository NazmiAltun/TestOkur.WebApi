namespace TestOkur.WebApi.Application.User.Events
{
    using TestOkur.Contracts;
    using TestOkur.Contracts.User;

    public class ResetPasswordTokenGenerated : IntegrationEvent, IResetPasswordTokenGenerated
	{
		public ResetPasswordTokenGenerated(string link, string email, string firstName, string lastName)
		{
			Link = link;
			Email = email;
			FirstName = firstName;
			LastName = lastName;
		}

		public string Link { get; }

		public string Email { get; }

		public string FirstName { get; }

		public string LastName { get; }
	}
}
