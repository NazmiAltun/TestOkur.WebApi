namespace TestOkur.WebApi.Application.Classroom
{
	using Dapper.FluentMap.Mapping;

	public class ClassroomReadModelMap : EntityMap<ClassroomReadModel>
	{
		public ClassroomReadModelMap()
		{
			Map(p => p.Name)
				.ToColumn("name_value");
			Map(p => p.Grade)
				.ToColumn("grade_value");
		}
	}
}
