namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.UserModel;

	internal class LicenseTypeEntityTypeConfiguration : IEntityTypeConfiguration<LicenseType>
    {
        public void Configure(EntityTypeBuilder<LicenseType> builder)
        {
            builder.ToTable("license_types");
            builder.Property(_ => _.Id)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasKey(_ => _.Id);
            builder.OwnsName(_ => _.Name, 150);
        }
    }
}
