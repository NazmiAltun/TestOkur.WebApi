namespace TestOkur.WebApi.Application.City
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Npgsql;
    using Paramore.Darker;
    using Paramore.Darker.QueryLogging;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Configuration;

    public sealed class GetAllCitiesQueryHandler : QueryHandlerAsync<GetAllCitiesQuery, IReadOnlyCollection<CityReadModel>>
    {
        private const string Sql = @"SELECT c.id,c.name_value as Name, d.id, d.name_value As districtname FROM cities c
                                 INNER JOIN districts d on c.id = d.city_id
                                 order by Name, districtname";

        private readonly string _connectionString;

        public GetAllCitiesQueryHandler(ApplicationConfiguration configurationOptions)
        {
            _connectionString = configurationOptions.Postgres;
        }

        [QueryLogging(1)]
        [ResultCaching(2)]
        public override async Task<IReadOnlyCollection<CityReadModel>> ExecuteAsync(GetAllCitiesQuery query, CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cityDictionary = new Dictionary<long, CityReadModel>();

                return (await connection.QueryAsync<CityReadModel, DistrictReadModel, CityReadModel>(
                    Sql,
                    (city, district) =>
                    {
                        if (!cityDictionary.TryGetValue(city.Id, out var cityEntry))
                        {
                            cityEntry = city;
                            cityDictionary.Add(cityEntry.Id, cityEntry);
                        }

                        cityEntry.Districts.Add(district);

                        return cityEntry;
                    }))
                    .Distinct()
                    .ToList();
            }
        }
    }
}
