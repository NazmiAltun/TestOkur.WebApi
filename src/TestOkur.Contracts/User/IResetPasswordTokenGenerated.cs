namespace TestOkur.Contracts.User
{
	public interface IResetPasswordTokenGenerated : IIntegrationEvent
	{
		string Link { get; }

		string Email { get; }

		string FirstName { get; }

		string LastName { get; }
	}
}
