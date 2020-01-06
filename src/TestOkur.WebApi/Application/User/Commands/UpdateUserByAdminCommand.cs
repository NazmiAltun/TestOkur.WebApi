namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using TestOkur.WebApi.Application.User.Clients;

    public class UpdateUserByAdminCommand : CommandBase, IClearCache
    {
        public UpdateUserByAdminCommand(
            Guid id,
            int updatedUserId,
            string firstName,
            string lastName,
            string subjectId,
            string schoolName,
            string mobilePhone,
            int cityId,
            int districtId,
            string email,
            int maxAllowedDeviceCount,
            int maxAllowedStudentCount,
            bool canScan,
            int licenseTypeId,
            DateTime? expiryDateUtc,
            string notes,
            bool active)
            : base(id)
        {
            UpdatedUserId = updatedUserId;
            FirstName = firstName;
            LastName = lastName;
            SubjectId = subjectId;
            SchoolName = schoolName;
            MobilePhone = mobilePhone;
            CityId = cityId;
            DistrictId = districtId;
            Email = email;
            MaxAllowedDeviceCount = maxAllowedDeviceCount;
            MaxAllowedStudentCount = maxAllowedStudentCount;
            CanScan = canScan;
            LicenseTypeId = licenseTypeId;
            ExpiryDateUtc = expiryDateUtc;
            Notes = notes;
            Active = active;
        }

        public UpdateUserByAdminCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { "Users" };

        public int UpdatedUserId { get; set; }

        public string SubjectId { get; set; }

        public string SchoolName { get; set; }

        public string MobilePhone { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int MaxAllowedDeviceCount { get; set; }

        public int MaxAllowedStudentCount { get; set; }

        public bool CanScan { get; set; }

        public int LicenseTypeId { get; set; }

        public DateTime? ExpiryDateUtc { get; set; }

        public bool Active { get; set; }

        public string Referrer { get; set; }

        public string Notes { get; set; }

        internal UpdateUserModel ToIdentityUpdateUserModel()
        {
            return new UpdateUserModel
            {
                CanScan = CanScan,
                Email = Email,
                ExpiryDateUtc = ExpiryDateUtc,
                LicenseTypeId = LicenseTypeId,
                MaxAllowedDeviceCount = MaxAllowedDeviceCount,
                MaxAllowedStudentCount = MaxAllowedStudentCount,
                UserId = SubjectId,
                Active = Active,
            };
        }
    }
}
