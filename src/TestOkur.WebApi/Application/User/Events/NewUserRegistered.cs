namespace TestOkur.WebApi.Application.User.Events
{
	using TestOkur.Contracts;
	using TestOkur.Contracts.User;

	public class NewUserRegistered : IntegrationEvent, INewUserRegistered
	{
		public NewUserRegistered(
			string email,
			string registrarFullName,
			string registrarPhone,
			string userFirstName,
			string userLastName,
			string schoolName,
			string userPhone,
			string licenseType,
			string district,
			string city)
		{
			Email = email;
			RegistrarFullName = registrarFullName;
			RegistrarPhone = registrarPhone;
			UserFirstName = userFirstName;
			UserLastName = userLastName;
			SchoolName = schoolName;
			UserPhone = userPhone;
			LicenseType = licenseType;
			District = district;
			City = city;
		}

		public string Email { get; }

		public string RegistrarFullName { get; }

		public string RegistrarPhone { get; }

		public string UserFirstName { get; }

		public string UserLastName { get; }

		public string SchoolName { get; }

		public string UserPhone { get; }

		public string LicenseType { get; }

		public string District { get; }

		public string City { get; }
	}
}
