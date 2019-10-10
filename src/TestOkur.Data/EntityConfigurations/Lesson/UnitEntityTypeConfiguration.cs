namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.LessonModel;

    internal class UnitEntityTypeConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("units");
            builder.Property(_ => _.Id)
                .UseHiLo("units_seq");
            builder.HasOne(_ => _.Lesson)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.Subjects)
                .WithOne()
                .HasForeignKey("unit_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.OwnsName(_ => _.Name, 150);
            builder.OwnsOne(_ => _.Grade);

            builder.Metadata.FindNavigation(nameof(Unit.Subjects))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
