namespace TestOkur.Sabit.Application.LicenseType
{
    using System.Collections.Generic;
    using TestOkur.Sabit.Infrastructure;

    public class LicenseTypeQuery : QueryWithCaching<IEnumerable<LicenseType>>
    {
        private LicenseTypeQuery()
        {
        }

        public static LicenseTypeQuery Default { get; } = new LicenseTypeQuery();
    }
}