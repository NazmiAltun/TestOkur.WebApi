namespace TestOkur.Data.EntityConfigurations.Score
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.ScoreModel;

	internal class ExamScoreFormulaEntityTypeConfiguration
		: IEntityTypeConfiguration<ExamScoreFormula>
	{
		public void Configure(EntityTypeBuilder<ExamScoreFormula> builder)
		{
			builder.HasBaseType<ScoreFormula>();
			builder.HasOne(_ => _.Exam)
				.WithMany()
				.HasForeignKey("exam_id")
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
