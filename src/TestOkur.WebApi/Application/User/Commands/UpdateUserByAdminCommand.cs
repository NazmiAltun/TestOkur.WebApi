namespace TestOkur.WebApi.Application.User.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using TestOkur.Infrastructure.Cqrs;
	using TestOkur.WebApi.Application.User.Services;

	[DataContract]
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
			string notes)
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
		}

		public IEnumerable<string> CacheKeys => new[] { "Users" };

		[DataMember]
		public int UpdatedUserId { get; private set; }

		[DataMember]
		public string SubjectId { get; private set; }

		[DataMember]
		public string SchoolName { get; private set; }

		[DataMember]
		public string MobilePhone { get; private set; }

		[DataMember]
		public int CityId { get; private set; }

		[DataMember]
		public int DistrictId { get; private set; }

		[DataMember]
		public string FirstName { get; private set; }

		[DataMember]
		public string LastName { get; private set; }

		[DataMember]
		public string Email { get; private set; }

		[DataMember]
		public int MaxAllowedDeviceCount { get; private set; }

		[DataMember]
		public int MaxAllowedStudentCount { get; private set; }

		[DataMember]
		public bool CanScan { get; private set; }

		[DataMember]
		public int LicenseTypeId { get; private set; }

		[DataMember]
		public DateTime? ExpiryDateUtc { get; private set; }

		[DataMember]
		public string Notes { get; private set; }

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
			};
		}
	}
}
