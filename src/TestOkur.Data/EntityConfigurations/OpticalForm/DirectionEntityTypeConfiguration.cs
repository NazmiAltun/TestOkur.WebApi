namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.OpticalFormModel;

    internal class DirectionEntityTypeConfiguration : IEntityTypeConfiguration<Direction>
    {
        public void Configure(EntityTypeBuilder<Direction> builder)
        {
            builder.ToTable("directions");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(_ => _.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
