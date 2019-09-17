namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.CityModel;

    internal class DistrictEntityTypeConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("districts");
            builder.Property(_ => _.Id)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasKey(_ => _.Id);
            builder.OwnsName(_ => _.Name, 150);
        }
    }
}
