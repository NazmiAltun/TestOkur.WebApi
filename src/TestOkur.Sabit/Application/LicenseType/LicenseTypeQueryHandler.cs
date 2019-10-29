namespace TestOkur.Sabit.Application.LicenseType
{
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Sabit.Utils;

    public class LicenseTypeQueryHandler : QueryHandlerAsync<LicenseTypeQuery, IEnumerable<LicenseType>>
    {
        [ResultCaching(1)]
        public override Task<IEnumerable<LicenseType>> ExecuteAsync(
            LicenseTypeQuery query,
            CancellationToken cancellationToken = default)
        {
            return JsonUtils.ReadAsync<IEnumerable<LicenseType>>(
                "license-types.json",
                cancellationToken);
        }
    }
}