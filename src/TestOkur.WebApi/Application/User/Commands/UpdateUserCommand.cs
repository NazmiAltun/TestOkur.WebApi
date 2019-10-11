namespace TestOkur.WebApi.Application.User.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TestOkur.Infrastructure.CommandsQueries;

    [DataContract]
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

        public IEnumerable<string> CacheKeys => new[] { "Users" };

        [DataMember]
        public string SchoolName { get; private set; }

        [DataMember]
        public string MobilePhone { get; private set; }

        [DataMember]
        public int CityId { get; private set; }

        [DataMember]
        public int DistrictId { get; private set; }
    }
}
