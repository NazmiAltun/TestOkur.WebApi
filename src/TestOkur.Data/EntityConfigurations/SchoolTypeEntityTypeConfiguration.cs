namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model;

    internal class SchoolTypeEntityTypeConfiguration : IEntityTypeConfiguration<SchoolType>
    {
        public void Configure(EntityTypeBuilder<SchoolType> builder)
        {
            builder.ToTable("school_types");

            builder.Property(_ => _.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(_ => _.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
