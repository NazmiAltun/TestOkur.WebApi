namespace TestOkur.WebApi.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;

    internal class SubjectSeeder : ISeeder
    {
        private const string FilePath = "shared-subjects.json";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            if (await dbContext.Units.AnyAsync(l => EF.Property<int>(l, "CreatedBy") == default))
            {
                return;
            }

            var rows = JsonConvert
                .DeserializeObject<List<SubjectUnitRow>>(
                    File.ReadAllText(Path.Combine("Data", FilePath)));

            var unitDictionary = new Dictionary<string, Unit>();
            var lessons = await dbContext.Lessons.
                Where(l => EF.Property<int>(l, "CreatedBy") == default)
                .ToListAsync();

            foreach (var row in rows)
            {
                try
                {
                    var unit = new Unit(
                        row.UnitName.Trim(),
                        lessons.First(l => l.Name.Value == row.Lesson.Trim()),
                        Convert.ToInt32(row.Grade.First().ToString()),
                        true);
                    unit.AddSubject(row.Subject.Trim(), true);

                    if (!unitDictionary.TryAdd(row.Key, unit))
                    {
                        unitDictionary[row.Key].AddSubject(row.Subject, true);
                    }
                }
                catch(Exception ex)
                {
                    throw;
                }
            }

            dbContext.Units.AddRange(unitDictionary.Values.ToList());
            await dbContext.SaveChangesAsync();
        }

        private class SubjectUnitRow
        {
            public string Subject { get; set; }

            public string UnitName { get; set; }

            public string Grade { get; set; }

            public string Lesson { get; set; }

            public string UnitNo { get; set; }

            [JsonIgnore]
            public string Key => $"{UnitName}_{Grade}_{Lesson}";
        }
    }
}
