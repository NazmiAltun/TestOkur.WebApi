namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Npgsql;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Domain.Model.ExamModel;
	using TestOkur.Domain.Model.LessonModel;
	using TestOkur.Domain.Model.OpticalFormModel;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.Domain.Model.SettingModel;
	using TestOkur.Domain.Model.StudentModel;
	using TestOkur.Domain.Model.UserModel;
	using TestOkur.Domain.SeedWork;
	using TestOkur.WebApi.Configuration;

	public static class DbInitializer
	{
		public static async Task CreateLogTableAsync(IServiceProvider services)
		{
			const string sql = @"CREATE TABLE IF NOT EXISTS request_response_logs(
								Id BIGSERIAL PRIMARY KEY,								
								request TEXT NULL,
								request_datetime_utc  timestamp without time zone NOT NULL,
								response TEXT NULL,
								response_datetime_utc  timestamp without time zone NOT NULL);";
			var connectionString = services.GetService<ApplicationConfiguration>()
				.Postgres;
			using (var connection = new NpgsqlConnection(connectionString))
			{
				await connection.OpenAsync();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = sql;
					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public static async Task SeedAsync(ApplicationDbContext context)
		{
			using (context)
			{
				await SeedSettingsAsync(context);
				await new CitySeeder(context).SeedAsync();
				await SeedLicenseTypesAsync(context);
				await new LessonSeeder(context).SeedAsync();
				await new OpticalFormsSeeder(context).SeedAsync();
				await SeedExamTypesAsync(context);
				await new UserSeeder(context).SeedAsync();
				await SeedScoreFormulas(context);
				await SeedEnumerationAsync(context);
			}
		}

		private static async Task SeedEnumerationAsync(ApplicationDbContext context)
		{
			if (!await context.Set<AnswerFormFormat>().AnyAsync())
			{
				context.Set<AnswerFormFormat>().AddRange(Enumeration.GetAll<AnswerFormFormat>());
				await context.SaveChangesAsync();
			}

			if (!await context.Set<ExamBookletType>().AnyAsync())
			{
				context.Set<ExamBookletType>().AddRange(Enumeration.GetAll<ExamBookletType>());
				await context.SaveChangesAsync();
			}

			if (!await context.Set<ContactType>().AnyAsync())
			{
				context.Set<ContactType>()
					.AddRange(Enumeration.GetAll<ContactType>());
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedScoreFormulas(ApplicationDbContext context)
		{
			if (await context.ScoreFormulas.AnyAsync())
			{
				return;
			}

			var forms = await context.FormTypes
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
				.AddLessonCoefficient(secondGradeFormType, Lessons.Language, 6)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(3)
				.WithBasePoint(200)
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Turkish, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Mathematics, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Hb, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Language, 5)
				.AddLessonCoefficient(thirdGradeFormType, Lessons.Science, 5)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithGrade(4)
				.WithBasePoint(200)
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Turkish, 5)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Mathematics, 5)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Language, 2)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.Science, 4)
				.AddLessonCoefficient(fourthGradeFormType, Lessons.SocialScience, 4)
				.Build());
			formulas.AddRange(new ScoreFormulaBuilder()
				.WithBasePoint(200)
				.SecondarySchool()
				.WithFormulaType(FormulaType.Trial)
				.AddLessonCoefficient(lgsFormType, Lessons.Turkish, 4)
				.AddLessonCoefficient(lgsFormType, Lessons.Mathematics, 4)
				.AddLessonCoefficient(lgsFormType, Lessons.Language, 2)
				.AddLessonCoefficient(lgsFormType, Lessons.Science, 4)
				.AddLessonCoefficient(lgsFormType, Lessons.SocialScience, 2)
				.AddLessonCoefficient(lgsFormType, Lessons.Religion, 2)
				.Build());

			// High School
			formulas.AddRange(new ScoreFormulaBuilder()
				.HighSchool()
				.WithFormulaType(FormulaType.TytAyt)
				.WithName(FormulaNames.Tyt)
				.WithBasePoint(100)
				.AddLessonCoefficient(tytFormType, Lessons.Turkish, 3.333f)
				.AddLessonCoefficient(tytFormType, Lessons.SocialScience, 3.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Mathematics, 3.333f)
				.AddLessonCoefficient(tytFormType, Lessons.Science, 3.333f)
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
				.AddLessonCoefficient(aytLangFormType, Lessons.Language, 3)
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

			context.ScoreFormulas.AddRange(formulas);
			await context.SaveChangesAsync();
		}

		private static async Task SeedSettingsAsync(ApplicationDbContext context)
		{
			if (!await context.AppSettings.AnyAsync())
			{
				var appSettings = new List<AppSetting>
				{
					new AppSetting(
					AppSettings.AdminEmails,
					"nazmialtun@windowslive.com;nazmialtun88@gmail.com",
					"Yönetici e-posta adresi.Her bir e-posta adresinden sonra ';' eklemek gerekiyor"),
					new AppSetting(
					AppSettings.AdminPhones,
					"5074011191;5052647544",
					"Yönetici telefon numaraları.Her bir numaradan sonra ';' eklemek gerekiyor."),
					new AppSetting(
					AppSettings.DailyJobRunTime,
					"23:55",
					"Sistem yoneticilerine 'TestOkur Gunluk Veriler' baslikli e-postayi gonderen isin calisma zamani."),
					new AppSetting(
					AppSettings.AccountExpirationNotificationTime,
					"20:00",
					"Kullanicilara 'TestOkur Lisans Süreniz Dolmak Üzere' baslikli e-posta ve smsi gonderen isin calisma zamani."),
					new AppSetting(
					AppSettings.AccountExpirationNotificationDayInterval,
					"7",
					"Kullanicilara gonderilen 'TestOkur Lisans Süreniz Dolmak Üzere' e-posta ve smsi lisans bitiminden kac gun once gonderilecek degerine karsilik gelir."),
					new AppSetting(
					AppSettings.SystemAdminEmails,
					"nazmialtun@windowslive.com;nazmialtun88@gmail.com",
					"Sistem yoneticisi e-posta adresi.Her bir e-posta adresinden sonra ';' eklemek gerekiyor"),
				};
				context.AppSettings.AddRange(appSettings);
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedExamTypesAsync(ApplicationDbContext context)
		{
			if (await context.ExamTypes.AnyAsync())
			{
				return;
			}

			var lessons = context.Lessons.Where(l => EF.Property<int>(l, "CreatedBy") == default).ToList();
			var trLesson = lessons.Single(l => l.Name.Value == Lessons.Turkish);
			var hbLesson = lessons.Single(l => l.Name.Value == Lessons.Hb);
			var matLesson = lessons.Single(l => l.Name.Value == Lessons.Mathematics);
			var scienceLesson = lessons.Single(l => l.Name.Value == Lessons.Science);
			var socLesson = lessons.Single(l => l.Name.Value == Lessons.SocialScience);
			var relLesson = lessons.Single(l => l.Name.Value == Lessons.Religion);
			var langLesson = lessons.Single(l => l.Name.Value == Lessons.Language);
			var litLesson = lessons.Single(l => l.Name.Value == Lessons.Literature);
			var formTypes = await context.FormTypes.ToListAsync();

			var singleLessonExam = new ExamTypeBuilder(ExamTypes.LessonExam)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABC))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCDE))
				.WithOrder(10)
				.Build();

			var evaluationExam = new ExamTypeBuilder(ExamTypes.EvaluationExam)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABC))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCD))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCDE))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCDE))
				.WithOrder(20)
				.Build();

			var lgsExam = new ExamTypeBuilder(ExamTypes.Lgs)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmLgs))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmLgsTwoPiece))
				.WithOrder(40)
				.Build();

			var tytExam = new ExamTypeBuilder(ExamTypes.Tyt)
				.WithDefaultIncorrectCorrectEliminationValue(4)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmTyt))
				.WithOrder(50)
				.Build();

			var aytLangExam = new ExamTypeBuilder(ExamTypes.AytLang)
				.WithDefaultIncorrectCorrectEliminationValue(4)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmAytLang))
				.WithOrder(70)
				.Build();

			var aytExam = new ExamTypeBuilder(ExamTypes.Ayt)
				.WithDefaultIncorrectCorrectEliminationValue(4)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmAyt))
				.WithOrder(60)
				.Build();

			var trialExam = new ExamTypeBuilder(ExamTypes.TrialExam)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm2ndGradeTrial))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm3rdGradeTrial))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm4thGradeTrial))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmTeog))
				.WithOrder(30)
				.Build();

			var scholarshipExam = new ExamTypeBuilder(ExamTypes.Scholarship)
				.WithDefaultIncorrectCorrectEliminationValue(3)
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmScholarship))
				.AddFormType(formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmScholarshipHigh))
				.WithOrder(50)
				.Build();

			context.Add(singleLessonExam);
			context.Add(evaluationExam);
			context.Add(lgsExam);
			context.Add(tytExam);
			context.Add(aytExam);
			context.Add(aytLangExam);
			context.Add(trialExam);
			context.Add(scholarshipExam);

			await context.SaveChangesAsync();
		}

		private static async Task SeedLicenseTypesAsync(ApplicationDbContext context)
		{
			if (await context.LicenseTypes.AnyAsync())
			{
				return;
			}

			var licenseTypes = new[]
			{
				new LicenseType(1, "İLKOKUL-ORTAOKUL – (BİREYSEL)", 1, 500, true),
				new LicenseType(2, "İLKOKUL-ORTAOKUL – (KURUMSAL)", 2, 99999, true),
				new LicenseType(3, "LİSE – (BİREYSEL)", 1, 500, true),
				new LicenseType(4, "LİSE – (KURUMSAL)", 2, 99999, true),
				new LicenseType(5, "İLK-ORTA + LİSE", 1, 500, true),
				new LicenseType(6, "SMS", 9999, 0, false),
			};

			context.LicenseTypes.AddRange(licenseTypes);
			await context.SaveChangesAsync();
		}
	}
}