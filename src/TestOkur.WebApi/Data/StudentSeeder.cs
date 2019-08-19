namespace TestOkur.WebApi.Data
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Data;
	using TestOkur.Domain.Model.ClassroomModel;
	using TestOkur.Domain.Model.StudentModel;

	public class StudentSeeder : ISeeder
	{
		private const string DefaultUserEmail = "nazmialtun@windowslive.com";
		private const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const int DefaultGrade = 8;
		private const string DefaultClassroomName = "B";

		private static readonly Random Random = new Random();

		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
		{
			var defaultUser = await dbContext.Users.FirstOrDefaultAsync(
				u => u.Email.Value == DefaultUserEmail);

			if (defaultUser == null)
			{
				return;
			}

			var defaultClassroom = await dbContext.Classrooms
				.FirstOrDefaultAsync(c => c.Grade.Value == DefaultGrade &&
										  c.Name.Value == DefaultClassroomName &&
										  EF.Property<int>(c, "CreatedBy") == defaultUser.Id);

			if (defaultClassroom != null)
			{
				return;
			}

			var classroom = new Classroom(DefaultGrade, DefaultClassroomName);
			dbContext.Classrooms.Add(classroom);
			dbContext.Entry(classroom).Property<int>("CreatedBy").CurrentValue = (int)defaultUser.Id;
			await dbContext.SaveChangesAsync();

			for (var i = 1; i < 10000; i++)
			{
				var student = new Student(
					RandomString(10),
					RandomString(10),
					i,
					classroom,
					new Contact[] { },
					RandomString(50));
				dbContext.Students.Add(student);
			}

			await dbContext.SaveChangesAsync();
		}

		private static string RandomString(int maxLength)
		{
			var length = 3 + Random.Next(maxLength - 3);
			return new string(Enumerable.Repeat(CharSet, length)
				.Select(s => s[Random.Next(s.Length)]).ToArray());
		}
	}
}
