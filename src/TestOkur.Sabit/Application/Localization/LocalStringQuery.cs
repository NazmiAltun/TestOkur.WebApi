namespace TestOkur.Sabit.Application.Localization
{
    using System.Collections.Generic;
    using TestOkur.Sabit.Infrastructure;

    public class LocalStringQuery : QueryWithCaching<IEnumerable<LocalString>>
    {
        private LocalStringQuery()
        {
        }

        public static LocalStringQuery Default { get; } = new LocalStringQuery();
    }
}