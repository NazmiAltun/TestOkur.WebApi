namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.CityModel;

	internal class CityEntityTypeConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("cities");
            builder.Property(_ => _.Id)
                .ValueGeneratedNever()
                .IsRequired();
            builder.HasKey(_ => _.Id);
            builder.OwnsName(_ => _.Name, 50);

            builder.HasMany(_ => _.Districts)
                .WithOne()
                .HasForeignKey("city_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(City.Districts))
                            .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}