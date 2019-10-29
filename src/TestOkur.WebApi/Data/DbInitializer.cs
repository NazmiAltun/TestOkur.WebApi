namespace TestOkur.WebApi.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TestOkur.Data;
    using TestOkur.WebApi.Application.City;
    using TestOkur.WebApi.Application.Exam;
    using TestOkur.WebApi.Application.Lesson;
    using TestOkur.WebApi.Application.OpticalForm;
    using TestOkur.WebApi.Application.Score;

    public static class DbInitializer
    {
        private static readonly IEnumerable<ISeeder> Seeders = new ISeeder[]
        {
            new CitySeeder(),
            new LessonSeeder(),
            new SubjectSeeder(),
            new OpticalFormsSeeder(),
            new ExamTypeSeeder(),
            new ScoreFormulaSeeder(),
            new EnumerationSeeder(),
        };

        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            using (context)
            {
                foreach (var seeder in Seeders)
                {
                    await seeder.SeedAsync(context, services);
                }
            }
        }
    }
}