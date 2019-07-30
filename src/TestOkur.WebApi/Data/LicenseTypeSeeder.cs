namespace TestOkur.WebApi.Data
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Data;
	using TestOkur.Domain.Model.UserModel;

	internal class LicenseTypeSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
		{
			if (await dbContext.LicenseTypes.AnyAsync())
			{
				return;
			}

			var licenseTypes = new[]
			{
				new LicenseType(1, "İLKOKUL-ORTAOKUL – (BİREYSEL)", 1, 500, true),
				new LicenseType(2, "İLKOKUL-ORTAOKUL – (KURUMSAL)", 2, 99999, true),
				new LicenseType(3, "LİSE – (BİREYSEL)", 1, 500, true),
				new LicenseType(4, "LİSE – (KURUMSAL)", 2, 99999, true),
				new LicenseType(5, "İLK-ORTA + LİSE", 1, 500, true),
				new LicenseType(6, "SMS", 9999, 0, false),
			};

			dbContext.LicenseTypes.AddRange(licenseTypes);
			await dbContext.SaveChangesAsync();
		}
	}
}
