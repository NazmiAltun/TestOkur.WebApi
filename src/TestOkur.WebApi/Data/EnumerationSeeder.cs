namespace TestOkur.WebApi.Data
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using TestOkur.Data;
    using TestOkur.Domain.Model.ExamModel;
    using TestOkur.Domain.Model.StudentModel;
    using TestOkur.Domain.SeedWork;

    internal class EnumerationSeeder : ISeeder
    {
	    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
	    {
		    if (!await dbContext.Set<AnswerFormFormat>().AnyAsync())
		    {
			    dbContext.Set<AnswerFormFormat>().AddRange(Enumeration.GetAll<AnswerFormFormat>());
			    await dbContext.SaveChangesAsync();
		    }

		    if (!await dbContext.Set<ExamBookletType>().AnyAsync())
		    {
			    dbContext.Set<ExamBookletType>().AddRange(Enumeration.GetAll<ExamBookletType>());
			    await dbContext.SaveChangesAsync();
		    }

		    if (!await dbContext.Set<ContactType>().AnyAsync())
		    {
			    dbContext.Set<ContactType>()
				    .AddRange(Enumeration.GetAll<ContactType>());
			    await dbContext.SaveChangesAsync();
		    }
		}
    }
}
