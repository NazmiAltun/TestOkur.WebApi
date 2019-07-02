namespace TestOkur.Data.EntityConfigurations.Score
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.ScoreModel;

	internal class FormulaTypeEntityTypeConfiguration
		: IEntityTypeConfiguration<FormulaType>
	{
		public void Configure(EntityTypeBuilder<FormulaType> builder)
		{
			builder.ToTable("formula_types");
			builder.HasKey(_ => _.Id);
			builder.Property(_ => _.Id)
				.ValueGeneratedNever()
				.IsRequired();
			builder.Property(_ => _.Name)
				.HasMaxLength(20)
				.IsRequired();
		}
	}
}
