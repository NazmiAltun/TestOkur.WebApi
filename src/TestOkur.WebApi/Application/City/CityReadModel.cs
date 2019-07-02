namespace TestOkur.WebApi.Application.City
{
	using System.Collections.Generic;

	public class CityReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<DistrictReadModel> Districts { get; set; } = new List<DistrictReadModel>();
    }
}
