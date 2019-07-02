namespace TestOkur.WebApi.Application.Student
{
    using Dapper.FluentMap.Mapping;

    public class StudentReadModelMap : EntityMap<StudentReadModel>
    {
        public StudentReadModelMap()
        {
            Map(p => p.FirstName)
                .ToColumn("first_name_value");
            Map(p => p.LastName)
                .ToColumn("last_name_value");
            Map(p => p.StudentNumber)
                .ToColumn("student_number_value");
            Map(p => p.ClassroomGrade)
                .ToColumn("grade_value");
            Map(p => p.ClassroomName)
                .ToColumn("name_value");
        }
    }
}
