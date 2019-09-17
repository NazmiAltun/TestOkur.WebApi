namespace TestOkur.WebApi.Application.LicenseType
{
    using System.Collections.Generic;
    using Paramore.Darker;

    public sealed class GetAllLicenseTypesQueryHandler : QueryHandler<GetAllLicenseTypesQuery, IReadOnlyCollection<LicenseTypeReadModel>>
    {
        public override IReadOnlyCollection<LicenseTypeReadModel> Execute(GetAllLicenseTypesQuery query)
        {
            return new[]
            {
                new LicenseTypeReadModel(1, "İLKOKUL-ORTAOKUL – (BİREYSEL)", 1000, 500, true),
                new LicenseTypeReadModel(2, "İLKOKUL-ORTAOKUL – (KURUMSAL)", 2, 99999, true),
                new LicenseTypeReadModel(3, "LİSE – (BİREYSEL)", 1, 500, true),
                new LicenseTypeReadModel(4, "LİSE – (KURUMSAL)", 2, 99999, true),
                new LicenseTypeReadModel(5, "İLK-ORTA + LİSE", 1, 500, true),
                new LicenseTypeReadModel(6, "SMS", 9999, 0, false),
            };
        }
    }
}
