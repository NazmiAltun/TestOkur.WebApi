namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using TestOkur.Infrastructure.CommandsQueries;

    public class UpdateUserCommand : CommandBase, IClearCache
    {
        public UpdateUserCommand(
            Guid id,
            string schoolName,
            string mobilePhone,
            int cityId,
            int districtId)
            : base(id)
        {
            SchoolName = schoolName;
            MobilePhone = mobilePhone;
            CityId = cityId;
            DistrictId = districtId;
        }

        public UpdateUserCommand()
        {
        }

        public IEnumerable<string> CacheKeys => new[] { "Users" };

        public string SchoolName { get; set; }

        public string MobilePhone { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }
    }
}
