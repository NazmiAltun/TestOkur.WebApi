namespace TestOkur.WebApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using TestOkur.Data;

    internal class SubjectSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            if (await dbContext.Units.AnyAsync(l => EF.Property<int>(l, "CreatedBy") == default))
            {
                return;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
