namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.OpticalFormModel;

	internal class OpticalFormDefinitionEntityTypeConfiguration : IEntityTypeConfiguration<OpticalFormDefinition>
    {
        public void Configure(EntityTypeBuilder<OpticalFormDefinition> builder)
        {
            builder.ToTable("optical_form_definitions");
            builder.Property(_ => _.Id)
                .ForNpgsqlUseSequenceHiLo("optical_form_definitions_seq");
            builder.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();
            builder.HasMany(_ => _.TextLocations)
                .WithOne()
                .HasForeignKey("optical_form_definition_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.SchoolType)
                .WithMany()
                .HasForeignKey("school_type_id");
            builder.Metadata.FindNavigation(nameof(OpticalFormDefinition.TextLocations))
                            .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
