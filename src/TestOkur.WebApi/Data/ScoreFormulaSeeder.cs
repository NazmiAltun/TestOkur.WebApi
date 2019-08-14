namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TestOkur.Data;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.Model.OpticalFormModel;
	using TestOkur.Domain.Model.ScoreModel;

	internal class ScoreFormulaSeeder : ISeeder
	{
	    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
	    {
			if (await dbContext.ScoreFormulas.AnyAsync())
			{
				return;
			}

			var forms = await dbContext.FormTypes
				.Include(f => f.FormLessonSections)
				.ThenInclude(l => l.Lesson)
				.ToListAsync();
			var secondGradeFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.Frm2ndGradeTrial);
			var thirdGradeFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.Frm3rdGradeTrial);
			var fourthGradeFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.Frm4thGradeTrial);
			var tytFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.FrmTyt);
			var aytFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.FrmAyt);
			var aytLangFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.FrmAytLang);
			var scholarshipFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.FrmScholarship);
			var lgsFormType = forms.First(f => f.Code == OpticalFormTypes.Codes.FrmLgs);
			var formulas = new List<ScoreFormula>();

			//Primary School
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(2)
				.WithBasePoint(200)
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(secondGradeFormType, Lessons.Turkish, 6)
				.AddLessonCoefficient(secondGradeFormType, Lessons.Mathematics, 6)
				.AddLessonCoefficient(secondGradeFormType, Lessons.Hb, 6)
				.AddLessonCoefficient(secondGradeFormType, Lessons.English, 6)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(3)
				.WithBasePoint(200)
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Turkish, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Mathematics, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Hb, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.English, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Science, 5)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(4)
				.WithBasePoint(200)
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Turkish, 5)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Mathematics, 5)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.English, 2)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Science, 4)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.SocialScience, 4)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithBasePoint(194.760f)
				.SecondarySchool()
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(lgsFormType, Lessons.Turkish, 3.388f)
				.AddLessonCoefficient(lgsFormType, Lessons.Mathematics, 5.005f)
				.AddLessonCoefficient(lgsFormType, Lessons.English, 1.769f)
				.AddLessonCoefficient(lgsFormType, Lessons.Science, 4.253f)
				.AddLessonCoefficient(lgsFormType, Lessons.SocialScience, 1.694f)
				.AddLessonCoefficient(lgsFormType, Lessons.Religion, 1.769f)
				.Build());

			// High School
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.Tyt)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 3.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 3.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 3.334f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 3.334f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.AytSoz)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 1.334f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 1.334f)
				.AddLessonCoefficient(aytFormType, Lessons.Literature, 3)
				.AddLessonCoefficient(aytFormType, Lessons.SocialScience, 3)
				.Build());

			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.AytSay)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 1.334f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 1.334f)
				.AddLessonCoefficient(aytFormType, Lessons.Mathematics, 3)
				.AddLessonCoefficient(aytFormType, Lessons.Science, 3)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.AytEa)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 1.334f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 1.334f)
				.AddLessonCoefficient(aytFormType, Lessons.Literature, 3)
				.AddLessonCoefficient(aytFormType, Lessons.Mathematics, 3)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.AytLang)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 1.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 1.334f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 1.334f)
				.AddLessonCoefficient(aytLangFormType, Lessons.English, 3)
				.Build());

			// Scholarship
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(4)
				.WithBasePoint(175)
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 4.33f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.296f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 2.601f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.773f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(5)
				.WithBasePoint(175)
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 4.33f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.296f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 2.601f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.773f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(6)
				.WithBasePoint(175)
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 4.397f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.569f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 2.641f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.393f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(7)
				.WithBasePoint(129)
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 4.721f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.898f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 3.691f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.530f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(8)
				.WithBasePoint(129)
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 4.291f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.998f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 3.881f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.670f)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithBasePoint(200)
				.WithName("Lise")
				.WithFormulaType(FormulaType.Scholarship)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Turkish, 3.5f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Mathematics, 3.5f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.Science, 2.5f)
				.AddLessonCoefficient(scholarshipFormType, Lessons.SocialScience, 2.5f)
				.Build());

			dbContext.ScoreFormulas.AddRange(formulas);
			await dbContext.SaveChangesAsync();
		}
    }
}
