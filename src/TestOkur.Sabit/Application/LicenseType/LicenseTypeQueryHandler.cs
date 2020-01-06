namespace TestOkur.Sabit.Application.LicenseType
{
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Serializer;

    public class LicenseTypeQueryHandler : QueryHandlerAsync<LicenseTypeQuery, IEnumerable<LicenseType>>
    {
        [ResultCaching(1)]
        public override Task<IEnumerable<LicenseType>> ExecuteAsync(
            LicenseTypeQuery query,
            CancellationToken cancellationToken = default)
        {
            return JsonUtils.DeserializeFromFileAsync<IEnumerable<LicenseType>>(
                Path.Join("Data", "license-types.json"),
                cancellationToken);
        }
    }
}