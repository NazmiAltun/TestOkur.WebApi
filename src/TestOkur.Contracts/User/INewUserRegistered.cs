namespace TestOkur.Contracts.User
{
	public interface INewUserRegistered : IIntegrationEvent
	{
		string Email { get; }

		string RegistrarFullName { get; }

		string RegistrarPhone { get; }

		string UserFirstName { get; }

		string UserLastName { get; }

		string SchoolName { get; }

		string UserPhone { get; }

		string LicenseType { get; }

		string District { get; }

		string City { get; }
	}
}
