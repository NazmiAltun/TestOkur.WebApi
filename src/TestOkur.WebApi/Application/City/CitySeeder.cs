namespace TestOkur.WebApi.Application.City
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using TestOkur.Data;
    using TestOkur.Domain.Model.CityModel;
    using TestOkur.WebApi.Data;

    internal class CitySeeder : ISeeder
    {
        private const string FilePath = "cities.json";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            if (await dbContext.Cities.AnyAsync())
            {
                return;
            }

            var cities = JsonConvert.DeserializeObject<List<CityReadModel>>(
                    File.ReadAllText(Path.Combine("Data", FilePath)))
                .Select(cr => new Domain.Model.CityModel.City(
                    cr.Id,
                    cr.Name,
                    cr.Districts.Select(d => new District(d.Id, d.Name))));

            dbContext.Cities.AddRange(cities.ToList());
            await dbContext.SaveChangesAsync();
        }
    }
}
