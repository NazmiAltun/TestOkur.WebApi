namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;
    using User = TestOkur.Domain.Model.UserModel.User;

    public class CreateUserCommand : CommandBase, IClearCache
    {
        public CreateUserCommand(
            Guid id,
            string registrarFullname,
            string registrarPhone,
            string userFirstName,
            string userLastName,
            string schoolName,
            string email,
            string userPhone,
            string password,
            int licenseTypeId,
            int cityId,
            int districtId,
            string licenseTypeName,
            string cityName,
            string districtName,
            Guid captchaId,
            string captchaCode)
        : base(id)
        {
            RegistrarFullName = registrarFullname;
            RegistrarPhone = registrarPhone;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            SchoolName = schoolName;
            Email = email;
            UserPhone = userPhone;
            Password = password;
            LicenseTypeId = licenseTypeId;
            CityId = cityId;
            DistrictId = districtId;
            CaptchaId = captchaId;
            CaptchaCode = captchaCode;
            LicenseTypeName = licenseTypeName;
            CityName = cityName;
            DistrictName = districtName;
        }

        public CreateUserCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };

        public string RegistrarFullName { get; set; }

        public string RegistrarPhone { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string SchoolName { get; set; }

        public string Email { get; set; }

        public string UserPhone { get; set; }

        public string Password { get; set; }

        public int LicenseTypeId { get; set; }

        public string LicenseTypeName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public Guid CaptchaId { get; set; }

        public string CaptchaCode { get; set; }

        public string Referrer { get; set; }

        public User ToDomainModel()
        {
            return new User(
                Id.ToString(),
                CityId,
                DistrictId,
                Email,
                UserPhone,
                UserFirstName,
                UserLastName,
                SchoolName,
                RegistrarFullName,
                RegistrarPhone,
                Referrer,
                null);
        }
    }
}
