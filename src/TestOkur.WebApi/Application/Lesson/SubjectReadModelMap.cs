namespace TestOkur.WebApi.Application.Lesson
{
    using Dapper.FluentMap.Mapping;

    public class SubjectReadModelMap : EntityMap<SubjectReadModel>
    {
        public SubjectReadModelMap()
        {
            Map(p => p.Id).ToColumn("subject_id");
            Map(p => p.Name).ToColumn("subject_name");
            Map(p => p.Shared).ToColumn("shared_subject");
        }
    }
}
