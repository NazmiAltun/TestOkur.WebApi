namespace TestOkur.Data.Extensions
{
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using TestOkur.Domain.SeedWork;

    internal static class EntityEntryExtensions
    {
        public static bool IsAuditable(this EntityEntry entry)
        {
            return typeof(IAuditable).IsAssignableFrom(entry.Metadata.ClrType);
        }
    }
}
