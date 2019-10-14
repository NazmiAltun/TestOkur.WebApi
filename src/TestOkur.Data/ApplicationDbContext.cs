namespace TestOkur.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
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
    using TestOkur.Infrastructure.Data;
    using TestOkur.Infrastructure.Mvc.Extensions;

    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext, ICanMigrate
    {
        private readonly int _currentUserId;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            int currentUserId = default)
           : base(options)
        {
            _currentUserId = currentUserId;
        }

        public DbSet<City> Cities { get; set; }

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
            AuditChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void Migrate()
        {
            Database.Migrate();
        }

        public async Task MigrateAsync()
        {
            await Database.MigrateAsync();
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

        private void AuditChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries().Where(e => e.IsAuditable()).ToList();

            foreach (var entry in entries)
            {
                SetCreationAttributes(entry);
                SetUpdateAttributes(entry);
            }
        }

        private void SetUpdateAttributes(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified ||
                entry.State == EntityState.Added)
            {
                entry.Property("UpdatedOnUTC").CurrentValue = DateTime.UtcNow;
                entry.Property("UpdatedBy").CurrentValue = _currentUserId;
            }
        }

        private void SetCreationAttributes(EntityEntry entry)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedOnUTC").CurrentValue = DateTime.UtcNow;
                entry.Property("CreatedBy").CurrentValue = _currentUserId;
            }
        }
    }
}
