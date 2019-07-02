namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.ExamModel;

	internal class ExamTypeEntityTypeConfiguration
		: IEntityTypeConfiguration<ExamType>
    {
        public void Configure(EntityTypeBuilder<ExamType> builder)
        {
            builder.ToTable("exam_types");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("exam_types_seq");
            builder.OwnsName(_ => _.Name, 100);
            builder.OwnsOne(_ => _.DefaultIncorrectEliminationRate);
            builder.HasMany(_ => _.ExamTypeOpticalFormTypes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(ExamType.ExamTypeOpticalFormTypes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
