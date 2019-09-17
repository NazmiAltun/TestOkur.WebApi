namespace TestOkur.WebApi.Application.Exam.Queries
{
    using Dapper.FluentMap.Mapping;

    public class ExamTypeReadModelMap : EntityMap<ExamTypeReadModel>
    {
        public ExamTypeReadModelMap()
        {
            Map(p => p.Name)
                .ToColumn("exam_type_name");

            Map(p => p.DefaultIncorrectEliminationRate)
                .ToColumn("default_incorrect_elimination_rate_value");
        }
    }
}
