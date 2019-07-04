namespace TestOkur.WebApi.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using TestOkur.Data;

	public static class DbInitializer
	{
		private static readonly IEnumerable<ISeeder> Seeders = new ISeeder[]
		{
			new SettingsSeeder(),
			new CitySeeder(),
			new LicenseTypeSeeder(),
			new LessonSeeder(),
			new OpticalFormsSeeder(),
			new ExamTypeSeeder(),
			new UserSeeder(),
			new ScoreFormulaSeeder(),
			new EnumerationSeeder(),
		};

		public static async Task SeedAsync(ApplicationDbContext context)
		{
			using (context)
			{
				foreach (var seeder in Seeders)
				{
					await seeder.SeedAsync(context);
				}
			}
		}
	}
}