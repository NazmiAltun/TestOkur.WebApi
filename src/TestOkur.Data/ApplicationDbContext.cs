namespace TestOkur.Data
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using TestOkur.Data.Extensions;
	using TestOkur.Domain.Model.CityModel;
	using TestOkur.Domain.Model.ClassroomModel;
	using TestOkur.Domain.Model.ExamModel;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.Model.OpticalFormModel;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.Domain.Model.SettingModel;
	using TestOkur.Domain.Model.StudentModel;
	using TestOkur.Domain.Model.UserModel;
	using TestOkur.Infrastructure;
	using TestOkur.Infrastructure.Extensions;

	[ExcludeFromCodeCoverage]
	public class ApplicationDbContext : DbContext
	{
		private readonly IUserIdProvider _userIdProvider;

		public ApplicationDbContext(
			DbContextOptions<ApplicationDbContext> options,
			IUserIdProvider userIdProvider)
           : base(options)
		{
			_userIdProvider = userIdProvider;
		}

		public DbSet<City> Cities { get; set; }

		public DbSet<LicenseType> LicenseTypes { get; set; }

		public DbSet<OpticalFormType> OpticalFormTypes { get; set; }

		public DbSet<ExamType> ExamTypes { get; set; }

		public DbSet<Exam> Exams { get; set; }

		public DbSet<Lesson> Lessons { get; set; }

		public DbSet<Unit> Units { get; set; }

		public DbSet<AppSetting> AppSettings { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Classroom> Classrooms { get; set; }

		public DbSet<Student> Students { get; set; }

		public DbSet<ScoreFormula> ScoreFormulas { get; set; }

		public DbSet<OpticalFormDefinition> OpticalFormDefinitions { get; set; }

		public DbSet<OpticalFormType> FormTypes { get; set; }

		public DbSet<ExamScanSession> ExamScanSessions { get; set; }

		public DbSet<Contact> Contacts { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AuditChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.AddAuditableProperties();
            modelBuilder.Entity<FormLessonSection>().Property<int>("ListOrder");
            modelBuilder.Entity<OpticalFormDefinition>().Property<int>("ListOrder");
            base.OnModelCreating(modelBuilder);
            modelBuilder.ToSnakeCase();
        }

		private async Task AuditChanges()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
			{
				if (!entry.IsAuditable())
				{
					continue;
				}

				await SetCreationAttributes(entry);
				await SetUpdateAttributes(entry);
			}
		}

		private async Task SetUpdateAttributes(EntityEntry entry)
		{
			if (entry.State == EntityState.Modified ||
			    entry.State == EntityState.Added)
			{
				entry.Property("UpdatedOnUTC").CurrentValue = DateTime.UtcNow;
				entry.Property("UpdatedBy").CurrentValue = await _userIdProvider.GetAsync();
			}
		}

		private async Task SetCreationAttributes(EntityEntry entry)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Property("CreatedOnUTC").CurrentValue = DateTime.UtcNow;
				entry.Property("CreatedBy").CurrentValue = await _userIdProvider.GetAsync();
			}
		}
	}
}
