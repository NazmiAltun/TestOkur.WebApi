namespace TestOkur.WebApi.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using TestOkur.Data;
    using TestOkur.Domain.Model.LessonModel;

    internal class LessonSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
        {
            var lessons = GetLessons();
            var existingLessons = await dbContext.Lessons.
                Where(l => EF.Property<int>(l, "CreatedBy") == default)
                .ToListAsync();
            foreach (var lesson in lessons)
            {
                if (!existingLessons.Contains(lesson))
                {
                    await dbContext.Lessons.AddAsync(lesson);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        private static IEnumerable<Lesson> GetLessons()
        {
            return typeof(Lessons)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .Select(s => new Lesson(s));
        }
    }
}
