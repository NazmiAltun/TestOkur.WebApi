namespace TestOkur.WebApi.Data
{
    using System;
    using System.Threading.Tasks;
    using TestOkur.Data;

    internal interface ISeeder
	{
		Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services);
	}
}
