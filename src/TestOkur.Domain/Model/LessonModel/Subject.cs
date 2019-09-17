namespace TestOkur.Domain.Model.LessonModel
{
    using TestOkur.Domain.SeedWork;

    public class Subject : Entity, IAuditable
	{
		public Subject(Name name)
		{
			Name = name;
		}

		protected Subject()
		{
		}

		public Name Name { get; private set; }

		public void SetName(string name)
		{
			Name = name;
		}
	}
}
