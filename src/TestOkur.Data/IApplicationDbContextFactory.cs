namespace TestOkur.Data
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext Create(int userId);
    }
}