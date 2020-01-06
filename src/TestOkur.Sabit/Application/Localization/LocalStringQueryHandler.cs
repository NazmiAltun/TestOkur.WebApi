namespace TestOkur.Sabit.Application.Localization
{
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Serializer;

    public class LocalStringQueryHandler : QueryHandlerAsync<LocalStringQuery, IEnumerable<LocalString>>
    {
        [ResultCaching(1)]
        public override Task<IEnumerable<LocalString>> ExecuteAsync(
            LocalStringQuery query,
            CancellationToken cancellationToken = default)
        {
            return JsonUtils.DeserializeFromFileAsync<IEnumerable<LocalString>>(
                Path.Join("Data", "local-strings.json"),
                cancellationToken);
        }
    }
}