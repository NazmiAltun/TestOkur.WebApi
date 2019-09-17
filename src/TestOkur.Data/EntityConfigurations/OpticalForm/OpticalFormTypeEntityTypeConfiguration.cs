namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.OpticalFormModel;

    internal class OpticalFormTypeEntityTypeConfiguration : IEntityTypeConfiguration<OpticalFormType>
    {
        public void Configure(EntityTypeBuilder<OpticalFormType> builder)
        {
            builder.ToTable("optical_form_types");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("optical_form_types_seq");
            builder.OwnsName(_ => _.Name, 100);
            builder.HasMany(_ => _.OpticalFormDefinitions)
                .WithOne()
                .HasForeignKey("optical_form_type_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(_ => _.FormLessonSections)
                .WithOne()
                .HasForeignKey("optical_form_type_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(OpticalFormType.OpticalFormDefinitions))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(OpticalFormType.FormLessonSections))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
