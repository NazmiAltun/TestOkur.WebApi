namespace TestOkur.Data.EntityConfigurations.Score
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.ScoreModel;

    internal class ScoreFormulaEntityTypeConfiguration
        : IEntityTypeConfiguration<ScoreFormula>
    {
        public void Configure(EntityTypeBuilder<ScoreFormula> builder)
        {
            builder.ToTable("score_formulas");
            builder.OwnsName(_ => _.Name, 20);
            builder.OwnsOne(_ => _.Grade);
            builder.HasMany(_ => _.Coefficients)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(ScoreFormula.Coefficients))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
