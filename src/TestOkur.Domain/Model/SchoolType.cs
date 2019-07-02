namespace TestOkur.Domain.Model
{
	using System.Collections.Generic;
	using TestOkur.Domain.SeedWork;

	public class SchoolType : Enumeration
	{
		public static readonly SchoolType PrimaryAndSecondary = new SchoolType(1, "Primary And Secondary");
		public static readonly SchoolType High = new SchoolType(2, "High");

		public SchoolType(int id, string name)
			: base(id, name)
		{
		}

		protected SchoolType()
		{
		}

		public IEnumerable<Grade> Grades =>
			this == High ? new Grade[] { 9, 10, 11, 12 } :
			new Grade[] { 1, 2, 3, 4, 5, 6, 7, 8 };
	}
}
