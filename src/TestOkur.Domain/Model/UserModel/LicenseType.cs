namespace TestOkur.Domain.Model.UserModel
{
	using TestOkur.Domain.SeedWork;

	public class LicenseType : Entity
    {
        public LicenseType(
            int id,
            Name name,
            int maxAllowedDeviceCount,
            int maxAllowedRecordCount,
            bool canScan)
        {
            Id = id;
            Name = name;
            MaxAllowedDeviceCount = maxAllowedDeviceCount;
            MaxAllowedRecordCount = maxAllowedRecordCount;
            CanScan = canScan;
        }

        protected LicenseType()
        {
        }

        public Name Name { get; private set; }

        public int MaxAllowedDeviceCount { get; private set; }

        public int MaxAllowedRecordCount { get; private set; }

        public bool CanScan { get; private set; }
    }
}
