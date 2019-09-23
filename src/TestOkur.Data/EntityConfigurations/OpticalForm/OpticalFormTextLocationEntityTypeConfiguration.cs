namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.OpticalFormModel;

    internal class OpticalFormTextLocationEntityTypeConfiguration : IEntityTypeConfiguration<OpticalFormTextLocation>
    {
        public void Configure(EntityTypeBuilder<OpticalFormTextLocation> builder)
        {
            builder.ToTable("optical_form_text_locations");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("optical_form_text_locations_seq");
            builder.OwnsOne(_ => _.Name);
            builder.OwnsOne(_ => _.Surname);
            builder.OwnsOne(_ => _.Class);
            builder.OwnsOne(_ => _.ExamName);
            builder.OwnsOne(_ => _.StudentNo);
            builder.OwnsOne(_ => _.StudentNoFillingPart);
            builder.OwnsOne(_ => _.CourseName);
            builder.OwnsOne(_ => _.Title1);
            builder.OwnsOne(_ => _.Title2);
        }
    }
}
