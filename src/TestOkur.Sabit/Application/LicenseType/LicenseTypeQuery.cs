namespace TestOkur.Sabit.Application.LicenseType
{
    using System.Collections.Generic;
    using TestOkur.Sabit.Infrastructure;

    public class LicenseTypeQuery : QueryWithCaching<IEnumerable<LicenseType>>
    {
        public static LicenseTypeQuery Default { get; } = new LicenseTypeQuery();
    }
}