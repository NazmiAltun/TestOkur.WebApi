namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using OfficeOpenXml;
	using TestOkur.Data;
	using TestOkur.Domain.Model.CityModel;
	using TestOkur.Domain.Model.UserModel;

	internal class UserSeeder : ISeeder
	{
		private const string UsersFilePath = "Users.xlsx";

		public async Task SeedAsync(ApplicationDbContext dbContext)
		{
			if (await dbContext.Users.AnyAsync())
			{
				return;
			}

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
					var city = cities.First(c => c.Id == Convert.ToInt32(workSheet.Cells[i, 20].Value));
					var district = city.Districts.First(d => d.Id == Convert.ToInt32(workSheet.Cells[i, 18].Value));
					var smsBalance = Convert.ToInt32(workSheet.Cells[i, 17].Value);

					list.Add(ParseFromRow(workSheet, i, city, district));

					if (smsBalance > 0)
					{
						list.Last().AddSmsBalance(smsBalance);
					}
				}
			}

			dbContext.Users.AddRange(list);
			await dbContext.SaveChangesAsync();
		}

		private User ParseFromRow(ExcelWorksheet workSheet, int rowIndex, City city, District district)
		{
			return new User(
				workSheet.Cells[rowIndex, 29].Value.ToString(),
				city,
				district,
				workSheet.Cells[rowIndex, 6].Value.ToString(),
				workSheet.Cells[rowIndex, 7].Value.ToString(),
				workSheet.Cells[rowIndex, 3].Value.ToString(),
				workSheet.Cells[rowIndex, 4].Value.ToString(),
				workSheet.Cells[rowIndex, 5].Value.ToString(),
				workSheet.Cells[rowIndex, 1].Value.ToString(),
				workSheet.Cells[rowIndex, 2].Value.ToString());
		}
	}
}
