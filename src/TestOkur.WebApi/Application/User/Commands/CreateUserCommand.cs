namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Domain.Model.CityModel;
    using TestOkur.Infrastructure.Cqrs;
    using City = TestOkur.Domain.Model.CityModel.City;
    using User = TestOkur.Domain.Model.UserModel.User;

    [DataContract]
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

        public IEnumerable<string> CacheKeys => new[] { "Users", "UserIdMap" };

        [DataMember]
        public string RegistrarFullName { get; private set; }

        [DataMember]
        public string RegistrarPhone { get; private set; }

        [DataMember]
        public string UserFirstName { get; private set; }

        [DataMember]
        public string UserLastName { get; private set; }

        [DataMember]
        public string SchoolName { get; private set; }

        [DataMember]
        public string Email { get; private set; }

        [DataMember]
        public string UserPhone { get; private set; }

        [DataMember]
        public string Password { get; private set; }

        [DataMember]
        public int LicenseTypeId { get; private set; }

        [DataMember]
        public string LicenseTypeName { get; private set; }

        [DataMember]
        public int CityId { get; private set; }

        [DataMember]
        public string CityName { get; private set; }

        [DataMember]
        public int DistrictId { get; private set; }

        [DataMember]
        public string DistrictName { get; private set; }

        [DataMember]
        public Guid CaptchaId { get; private set; }

        [DataMember]
        public string CaptchaCode { get; private set; }

        public User ToDomainModel(City city, District district)
        {
            return new User(
                Id.ToString(),
                city,
                district,
                Email,
                UserPhone,
                UserFirstName,
                UserLastName,
                SchoolName,
                RegistrarFullName,
                RegistrarPhone,
                null);
        }
    }
}
