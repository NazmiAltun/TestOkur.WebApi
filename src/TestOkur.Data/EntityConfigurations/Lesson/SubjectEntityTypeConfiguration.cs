namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.LessonModel;

    internal class SubjectEntityTypeConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("subjects");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("subjects_seq");
            builder.OwnsName(_ => _.Name, 300);
        }
    }
}
