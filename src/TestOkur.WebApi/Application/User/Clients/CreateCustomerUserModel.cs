namespace TestOkur.WebApi.Application.User.Clients
{
    public class CreateCustomerUserModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int MaxAllowedDeviceCount { get; set; }

        public int MaxAllowedStudentCount { get; set; }

        public bool CanScan { get; set; }

        public int LicenseTypeId { get; set; }
    }
}
