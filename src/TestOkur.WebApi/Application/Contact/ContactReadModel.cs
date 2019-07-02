namespace TestOkur.WebApi.Application.Contact
{
	public class ContactReadModel
	{
		public int Id { get; set; }

		public string Phone { get; set; }

		public int ContactType { get; set; }

		public string ContactTypeName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Labels { get; set; }

		public string ClassroomName { get; set; }

		public int Grade { get; set; }
	}
}
