namespace TestOkur.Domain.Model.ExamModel
{
    using TestOkur.Domain.SeedWork;

    public class ExamBookletType : Enumeration
	{
		public static readonly ExamBookletType Single = new ExamBookletType(1, "Tek Kitapçık");
		public static readonly ExamBookletType AB = new ExamBookletType(2, "A - B");
		public static readonly ExamBookletType ABC = new ExamBookletType(3, "A - B - C");
		public static readonly ExamBookletType ABCD = new ExamBookletType(4, "A - B - C - D");

		public ExamBookletType(int id, string name)
			: base(id, name)
		{
		}

		protected ExamBookletType()
		{
		}
	}
}
