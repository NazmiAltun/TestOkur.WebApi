namespace TestOkur.WebApi.Application.Lesson
{
	using System.Collections.Generic;

	public class UnitReadModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int LessonId { get; set; }

		public string Lesson { get; set; }

		public int Grade { get; set; }

		public List<SubjectReadModel> Subjects { get; set; }
			= new List<SubjectReadModel>();
	}
}
