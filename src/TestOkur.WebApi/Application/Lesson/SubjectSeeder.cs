// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace TestOkur.WebApi.Application.Lesson
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;
    using TestOkur.Infrastructure.Mvc.Helpers;
    using TestOkur.WebApi.Data;

    internal class SubjectSeeder : ISeeder
    {
        private const string FilePath = "shared-subjects.json";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            if (await dbContext.Units
                .AnyAsync(l => EF.Property<int>(l, "CreatedBy") == default &&
                               l.Lesson.Name.Value == Lessons.Religion))
            {
                return;
            }

            var rows = JsonConvert
                .DeserializeObject<List<SubjectUnitRow>>(
                    await FileEx.ReadAllTextAsync(Path.Combine("Data", FilePath)));

            var unitDictionary = new Dictionary<string, Unit>();
            var lessons = await dbContext.Lessons.
                Where(l => EF.Property<int>(l, "CreatedBy") == default)
                .ToListAsync();

            foreach (var row in rows)
            {
                var unit = new Unit(
                        row.UnitName.Trim(),
                        lessons.First(l => l.Name.Value == row.Lesson.Trim()),
                        Convert.ToInt32(row.Grade.First().ToString()),
                        true);

                if (unit.Lesson.Name.Value != Lessons.Religion)
                {
                    continue;
                }

                unit.AddSubject(row.Subject.Trim(), true);

                if (!unitDictionary.TryAdd(row.Key, unit))
                {
                    unitDictionary[row.Key].AddSubject(row.Subject, true);
                }
            }

            dbContext.Units.AddRange(unitDictionary.Values.ToList());
            await dbContext.SaveChangesAsync();
        }

#pragma warning disable S3459 // Unassigned members should be removed
        private class SubjectUnitRow
        {
            public string Subject { get; set; }

            public string UnitName { get; set; }

            public string Grade { get; set; }

            public string Lesson { get; set; }

            [JsonIgnore]
            public string Key => $"{UnitName}_{Grade}_{Lesson}";
        }
#pragma warning restore S3459 // Unassigned members should be removed

    }
}
