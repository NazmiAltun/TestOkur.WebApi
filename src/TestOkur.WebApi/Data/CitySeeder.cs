namespace TestOkur.WebApi.Data
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Domain.Model.CityModel;
    using TestOkur.WebApi.Application.City;

    internal class CitySeeder : ISeeder
    {
        private const string CityExcelFilePath = "cities.json";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            if (await dbContext.Cities.AnyAsync())
            {
                return;
            }

            var cities = JsonConvert.DeserializeObject<List<CityReadModel>>(
                    File.ReadAllText(Path.Combine("Data", CityExcelFilePath)))
                .Select(cr => new City(
                    cr.Id,
                    cr.Name,
                    cr.Districts.Select(d => new District(d.Id, d.Name))));

            dbContext.Cities.AddRange(cities.ToList());
            await dbContext.SaveChangesAsync();
        }
    }
}
