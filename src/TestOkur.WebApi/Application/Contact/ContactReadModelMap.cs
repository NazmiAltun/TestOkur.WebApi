namespace TestOkur.WebApi.Application.Contact
{
    using Dapper.FluentMap.Mapping;

    public class ContactReadModelMap : EntityMap<ContactReadModel>
	{
		public ContactReadModelMap()
		{
			Map(p => p.FirstName).ToColumn("first_name_value");
			Map(p => p.LastName).ToColumn("last_name_value");
			Map(p => p.Phone).ToColumn("phone_value");
			Map(p => p.ContactType).ToColumn("contact_type_id");
		}
	}
}
