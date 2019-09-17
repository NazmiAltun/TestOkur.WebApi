namespace TestOkur.Data.EntityConfigurations.Exam
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.ExamModel;

    internal class ExamEntityTypeConfiguration
        : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("exams");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("exams_seq");
            builder.OwnsName(_ => _.Name, 150);
            builder.OwnsOne(_ => _.IncorrectEliminationRate);
            builder.HasOne(_ => _.ExamType)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey("exam_type_id");
            builder.HasOne(_ => _.Lesson)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey("lesson_id");
            builder.HasOne(_ => _.AnswerFormFormat)
                .WithMany()
                .HasForeignKey("answer_form_format_id");
        }
    }
}
