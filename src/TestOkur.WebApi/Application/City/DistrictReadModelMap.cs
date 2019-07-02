namespace TestOkur.WebApi.Application.City
{
	using Dapper.FluentMap.Mapping;

	public class DistrictReadModelMap : EntityMap<DistrictReadModel>
    {
        public DistrictReadModelMap()
        {
            Map(p => p.Name)
                .ToColumn("districtname");
        }
    }
}
