namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.ClassroomModel;

    internal class ClassroomEntityTypeConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            builder.ToTable("classrooms");
            builder.OwnsOne(_ => _.Grade);
            builder.OwnsName(_ => _.Name, 50);
        }
    }
}
