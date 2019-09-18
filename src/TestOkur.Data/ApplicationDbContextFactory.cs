namespace TestOkur.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContextFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public ApplicationDbContext Create(int userId)
        {
            return new ApplicationDbContext(_options, userId);
        }
    }
}
