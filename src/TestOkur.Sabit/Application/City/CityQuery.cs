namespace TestOkur.Sabit.Application.City
{
    using System.Collections.Generic;
    using TestOkur.Sabit.Infrastructure;

    public class CityQuery : QueryWithCaching<IEnumerable<City>>
    {
        private CityQuery()
        {
        }

        public static CityQuery Default { get; } = new CityQuery();
    }
}
