namespace TestOkur.Data.EntityConfigurations.Exam
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.ExamModel;

    internal class ExamTypeOpticalFormTypeEntityTypeConfiguration
        : IEntityTypeConfiguration<ExamTypeOpticalFormType>
    {
        public void Configure(EntityTypeBuilder<ExamTypeOpticalFormType> builder)
        {
            builder.ToTable("exam_type_optical_form_types");
            builder.Property(_ => _.Id)
                .UseHiLo("exam_type_optical_form_seq");
            builder.HasOne(_ => _.OpticalFormType)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("optical_form_type_id");
        }
    }
}
