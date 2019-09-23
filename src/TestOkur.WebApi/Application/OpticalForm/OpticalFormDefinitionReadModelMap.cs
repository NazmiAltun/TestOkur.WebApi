namespace TestOkur.WebApi.Application.OpticalForm
{
    using Dapper.FluentMap.Mapping;

    public class OpticalFormDefinitionReadModelMap : EntityMap<OpticalFormDefinitionReadModel>
    {
        public OpticalFormDefinitionReadModelMap()
        {
            Map(p => p.Id)
                .ToColumn("opt_id");

            Map(p => p.SchoolType)
                .ToColumn("school_type_id");

            Map(p => p.TextDirection)
                .ToColumn("text_direction_id");

            Map(p => p.StudentNumberFillDirection)
                .ToColumn("student_number_fill_direction_id");
        }
    }
}
