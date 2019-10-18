namespace TestOkur.WebApi.Application.Localization
{
    using Newtonsoft.Json;
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.IO;
    using TestOkur.Infrastructure.CommandsQueries;

    public class GetLocalStringsQueryHandler : QueryHandler<GetLocalStringsQuery, IReadOnlyCollection<LocalString>>
    {
        [ResultCaching(2)]
        public override IReadOnlyCollection<LocalString> Execute(GetLocalStringsQuery query)
        {
            return JsonConvert.DeserializeObject<IReadOnlyCollection<LocalString>>(
                File.ReadAllText(Path.Combine(
                    "Application", "Localization", $"{query.CultureCode}.json")));
        }
    }
}
