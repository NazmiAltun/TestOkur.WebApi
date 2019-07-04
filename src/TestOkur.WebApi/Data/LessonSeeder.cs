namespace TestOkur.WebApi.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Data;
	using TestOkur.Domain.Model.LessonModel;

	internal class LessonSeeder : ISeeder
	{
		private readonly ApplicationDbContext _dbContext;

		public LessonSeeder(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SeedAsync()
		{
			if (await _dbContext.Lessons.AnyAsync(l => EF.Property<int>(l, "CreatedBy") == default))
			{
				return;
			}

			_dbContext.Lessons.AddRange(GetLessons());
			await _dbContext.SaveChangesAsync();
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
