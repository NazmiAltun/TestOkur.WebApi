namespace TestOkur.WebApi.Application.User.Clients
{
    public class LicenseType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MaxAllowedDeviceCount { get; set; }

        public int MaxAllowedRecordCount { get; set; }

        public bool CanScan { get; set; }
    }
}