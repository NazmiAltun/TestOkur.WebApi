namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.OpticalFormModel;

    internal class FormLessonSectionEntityTypeConfiguration : IEntityTypeConfiguration<FormLessonSection>
    {
        public void Configure(EntityTypeBuilder<FormLessonSection> builder)
        {
            builder.ToTable("form_lesson_sections");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("form_lesson_sections_seq");
            builder.HasOne(_ => _.Lesson)
                .WithMany()
                .HasForeignKey("lesson_id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
