namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.StudentModel;

	internal class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Contact>
	{
		public void Configure(EntityTypeBuilder<Contact> builder)
		{
			builder.ToTable("contacts");
			builder.OwnsPhone(_ => _.Phone);
			builder.OwnsOne(_ => _.FirstName);
			builder.OwnsOne(_ => _.LastName);
			builder.HasOne(o => o.ContactType)
				.WithMany()
				.HasForeignKey("contact_type_id");
		}
	}
}
