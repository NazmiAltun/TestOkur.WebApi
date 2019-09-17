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

    internal class CitySeeder : ISeeder
	{
		private const string CityExcelFilePath = "cities-districts.xlsx";

		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
		{
			if (await dbContext.Cities.AnyAsync())
			{
				return;
			}

			var cityDict = new Dictionary<long, City>();

			using (var package = GetExcelPackage())
			{
				var workSheet = package.Workbook.Worksheets.First();

				for (var i = 2; i < workSheet.Dimension.Rows + 1; i++)
				{
					var city = ParseCity(workSheet, i);
					cityDict.TryAdd(city.Id, city);
					AddDistrict(workSheet, i, cityDict, city);
				}
			}

			await AddAsync(cityDict.Values, dbContext);
		}

		private void AddDistrict(ExcelWorksheet workSheet, int i, Dictionary<long, City> cityDict, City city)
		{
			var districtId = Convert.ToInt32(workSheet.Cells[i, 3].Value);
			var districtName = workSheet.Cells[i, 4].Value.ToString();
			cityDict[city.Id].AddDistrict(districtId, districtName);
		}

		private ExcelPackage GetExcelPackage()
		{
			var file = new FileInfo(Path.Combine("Data", CityExcelFilePath));
			return new ExcelPackage(file);
		}

		private City ParseCity(ExcelWorksheet workSheet, int rowIndex)
		{
			var cityId = Convert.ToInt32(workSheet.Cells[rowIndex, 1].Value);
			var cityName = workSheet.Cells[rowIndex, 2].Value.ToString();

			return new City(cityId, cityName);
		}

		private async Task AddAsync(IEnumerable<City> cities, ApplicationDbContext dbContext)
		{
			dbContext.Cities.AddRange(cities.ToList());
			await dbContext.SaveChangesAsync();
		}
	}
}
