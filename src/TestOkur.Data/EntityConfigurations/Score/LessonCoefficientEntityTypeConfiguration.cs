namespace TestOkur.Data.EntityConfigurations.Score
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.ScoreModel;

	internal class LessonCoefficientEntityTypeConfiguration
		: IEntityTypeConfiguration<LessonCoefficient>
	{
		public void Configure(EntityTypeBuilder<LessonCoefficient> builder)
		{
			builder.ToTable("lesson_coefficients");
			builder.HasOne(_ => _.ExamLessonSection)
				.WithMany()
				.HasForeignKey("exam_lesson_section_id");
		}
	}
}
