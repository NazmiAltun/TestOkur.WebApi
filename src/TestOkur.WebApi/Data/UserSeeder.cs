namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using OfficeOpenXml;
	using TestOkur.Data;
	using TestOkur.Domain.Model.CityModel;
	using TestOkur.Domain.Model.UserModel;

	internal class UserSeeder : ISeeder
	{
		private const string UsersFilePath = "Users.xlsx";

		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
		{
			var list = new List<User>();
			var file = new FileInfo(Path.Combine("Data", UsersFilePath));

			using (var package = new ExcelPackage(file))
			{
				var workSheet = package.Workbook.Worksheets.First();
				var cities = dbContext.Cities
					.Include(c => c.Districts)
					.ToList();
				for (var i = 2; i < workSheet.Dimension.Rows + 1; i++)
				{
					try
					{
						var city = cities.First(c => c.Id == Convert.ToInt32(workSheet.Cells[i, 2].Value));
						var district = city.Districts.First(d => d.Id == Convert.ToInt32(workSheet.Cells[i, 3].Value));
						var smsBalance = Convert.ToInt32(workSheet.Cells[i, 11].Value);

						list.Add(ParseFromRow(workSheet, i, city, district));

						if (smsBalance > 0)
						{
							list.Last().AddSmsBalance(smsBalance);
						}
					}
					catch (Exception ex)
					{
						services.GetService<ILogger<UserSeeder>>()
							.LogError(ex, $"Unable to parse user at row {i}");
					}
				}
			}

			foreach (var user in list)
			{
				if (await dbContext.Users.AnyAsync(u => u.Email.Value == user.Email.Value))
				{
					continue;
				}

				dbContext.Users.Add(user);
			}

			await dbContext.SaveChangesAsync();
		}

		private User ParseFromRow(ExcelWorksheet workSheet, int rowIndex, City city, District district)
		{
			return new User(
				workSheet.Cells[rowIndex, 1].Value.ToString(),
				city,
				district,
				workSheet.Cells[rowIndex, 4].Value.ToString(),
				workSheet.Cells[rowIndex, 5].Value.ToString(),
				workSheet.Cells[rowIndex, 6].Value.ToString(),
				workSheet.Cells[rowIndex, 7].Value.ToString(),
				workSheet.Cells[rowIndex, 8].Value.ToString(),
				workSheet.Cells[rowIndex, 9].Value.ToString(),
				workSheet.Cells[rowIndex, 10].Value.ToString());
		}
	}
}
