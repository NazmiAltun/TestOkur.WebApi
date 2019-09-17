namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.StudentModel;

    internal class ContactTypeEntityTypeConfiguration :
		IEntityTypeConfiguration<ContactType>
	{
		public void Configure(EntityTypeBuilder<ContactType> builder)
		{
			builder.ToTable("contact_types");

			builder.Property(_ => _.Id)
				.ValueGeneratedNever()
				.IsRequired();

			builder.Property(_ => _.Name)
				.HasMaxLength(20)
				.IsRequired();
		}
	}
}
