namespace TestOkur.Data.EntityConfigurations
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using TestOkur.Domain.Model.UserModel;

	internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("users");
			builder.Property(_ => _.Id)
				.ForNpgsqlUseSequenceHiLo("user_seq");
			builder.HasOne(_ => _.City)
				.WithMany()
				.OnDelete(DeleteBehavior.SetNull);
			builder.HasOne(_ => _.District)
				.WithMany()
				.OnDelete(DeleteBehavior.SetNull);
			builder.Property(_ => _.SubjectId).IsRequired();
			builder.OwnsEmail(_ => _.Email);
			builder.OwnsName(_ => _.FirstName, 100);
			builder.OwnsName(_ => _.LastName, 100);
			builder.OwnsName(_ => _.RegistrarFullName, 200);
			builder.OwnsName(_ => _.SchoolName, 150);
			builder.OwnsPhone(_ => _.Phone);
			builder.OwnsPhone(_ => _.RegistrarPhone);
			builder.Property(_ => _.Notes).HasMaxLength(500);
		}
	}
}
