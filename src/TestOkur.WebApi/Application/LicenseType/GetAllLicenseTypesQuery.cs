namespace TestOkur.WebApi.Application.LicenseType
{
    using System.Collections.Generic;
    using Paramore.Darker;

    public sealed class GetAllLicenseTypesQuery :
        IQuery<IReadOnlyCollection<LicenseTypeReadModel>>
	{
    }
}
