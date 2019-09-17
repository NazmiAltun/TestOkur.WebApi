namespace TestOkur.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TestOkur.Domain.Model.StudentModel;

    internal class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
	{
		public void Configure(EntityTypeBuilder<Student> builder)
		{
			builder.ToTable("students");
			builder.OwnsName(_ => _.FirstName, 50);
			builder.OwnsName(_ => _.LastName, 50);
			builder.OwnsOne(u => u.StudentNumber);
			builder.Property(_ => _.Notes).HasMaxLength(500);
			builder.HasMany(_ => _.Contacts)
				.WithOne()
				.HasForeignKey("student_id")
				.OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(_ => _.Classroom)
				.WithMany()
				.HasForeignKey("classroom_id")
				.OnDelete(DeleteBehavior.Cascade);
			builder.Metadata.FindNavigation(nameof(Student.Contacts))
				.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
