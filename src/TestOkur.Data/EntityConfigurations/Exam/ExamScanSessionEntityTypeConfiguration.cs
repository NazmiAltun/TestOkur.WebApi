namespace TestOkur.Data.EntityConfigurations.Exam
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.ExamModel;

    internal class ExamScanSessionEntityTypeConfiguration
        : IEntityTypeConfiguration<ExamScanSession>
    {
        public void Configure(EntityTypeBuilder<ExamScanSession> builder)
        {
            builder.ToTable("exam_scan_sessions");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.HasOne(_ => _.Exam)
                .WithMany()
                .HasForeignKey("exam_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
