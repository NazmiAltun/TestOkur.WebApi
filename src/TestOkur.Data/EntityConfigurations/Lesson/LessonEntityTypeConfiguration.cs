namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.LessonModel;

    internal class LessonEntityTypeConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("lessons");
            builder.Property(_ => _.Id)
             .ForNpgsqlUseSequenceHiLo("lessons_seq");
            builder.OwnsName(_ => _.Name, 50);
        }
    }
}