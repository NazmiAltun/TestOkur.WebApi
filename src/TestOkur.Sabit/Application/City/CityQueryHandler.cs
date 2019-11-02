namespace TestOkur.Sabit.Application.City
{
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Sabit.Utils;

    public class CityQueryHandler : QueryHandlerAsync<CityQuery, IEnumerable<City>>
    {
        [ResultCaching(1)]
        public override Task<IEnumerable<City>> ExecuteAsync(
            CityQuery query,
            CancellationToken cancellationToken = default)
        {
            return JsonUtils.ReadAsync<IEnumerable<City>>(
                "cities.json",
                cancellationToken);
        }
    }
}