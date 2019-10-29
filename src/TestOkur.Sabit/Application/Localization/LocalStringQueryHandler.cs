namespace TestOkur.Sabit.Application.Localization
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Darker;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.Sabit.Utils;

    public class LocalStringQueryHandler : QueryHandlerAsync<LocalStringQuery, IEnumerable<LocalString>>
    {
        [ResultCaching(1)]
        public override Task<IEnumerable<LocalString>> ExecuteAsync(
            LocalStringQuery query,
            CancellationToken cancellationToken = default)
        {
            return JsonUtils.ReadAsync<IEnumerable<LocalString>>(
                "local-strings.json",
                cancellationToken);
        }
    }
}