namespace TestOkur.Domain.Model.StudentModel
{
	using TestOkur.Domain.SeedWork;

	public class ContactType : Enumeration
	{
		public static readonly ContactType Mother = new ContactType(1, "Mother");
		public static readonly ContactType Father = new ContactType(2, "Father");
		public static readonly ContactType Directory = new ContactType(3, "Directory");

		public ContactType(int id, string name)
			: base(id, name)
		{
		}

		protected ContactType()
		{
		}
	}
}
