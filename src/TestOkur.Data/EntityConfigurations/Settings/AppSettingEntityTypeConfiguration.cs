namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.SettingModel;

    internal class AppSettingEntityTypeConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.ToTable("appsettings");
            builder.Property(_ => _.Id)
               .ForNpgsqlUseSequenceHiLo("appsettings_seq");
            builder.OwnsName(_ => _.Name, 100);
            builder.Property(_ => _.Value)
                .IsUnicode()
                .IsRequired();
        }
    }
}
