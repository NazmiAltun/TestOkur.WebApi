namespace TestOkur.WebApi.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Npgsql;
	using TestOkur.Common;
	using TestOkur.Data;
	using TestOkur.Domain.Model;
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
				await SeedOpticalFormsAsync(context);
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

		[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
		private static async Task SeedOpticalFormsAsync(ApplicationDbContext context)
		{
			if (await context.FormTypes.AnyAsync())
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

			var formDefinitions = new List<OpticalFormDefinition>
				{
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Abcd20)
					.SetStudentNoXInterval(53)
					.SetStudentNoFillWidth(45)
					.SetStudentNoYInterval(62)
					.SetDescription("20 - Soruluk Optik Form (ABCD Seçenekli)")
					.PrimarySchool()
					.SetFilename("20 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToLeft)
					.SetTextDirection(Direction.ToBottom)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(290, 525)
						.SetSurnameLocation(225, 525)
						.SetClassLocation(160, 525)
						.SetStudentNoLocation(160, 1065)
						.SetStudentNoFillingPartLocation(963, 419)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1480, 525)
						.SetSurnameLocation(1415, 525)
						.SetClassLocation(1350, 525)
						.SetStudentNoLocation(1350, 1065)
						.SetStudentNoFillingPartLocation(2159, 419)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(290, 2290)
						.SetSurnameLocation(225, 2290)
						.SetClassLocation(160, 2290)
						.SetStudentNoLocation(160, 2835)
						.SetStudentNoFillingPartLocation(963, 2182)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1480, 2290)
						.SetSurnameLocation(1415, 2290)
						.SetClassLocation(1350, 2290)
						.SetStudentNoLocation(1350, 2835)
						.SetStudentNoFillingPartLocation(2159, 2182)
						.Build())
					.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Abcd30)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("30 - Soruluk Optik Form (ABCD Seçenekli)")
					.PrimarySchool()
					.SetFilename(@"30 Soruluk - ABCD.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1600, 1000)
						.SetSurnameLocation(1665, 1000)
						.SetClassLocation(1730, 1000)
						.SetExamNameLocation(1795, 1000)
						.SetStudentNoFillingPartLocation(630, 500)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1610, 2780)
						.SetSurnameLocation(1675, 2780)
						.SetClassLocation(1740, 2780)
						.SetExamNameLocation(1800, 2780)
						.SetStudentNoFillingPartLocation(630, 2270)
						.Build())
					.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Abcd60)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("60 - Soruluk Optik Form (ABCD Seçenekli)")
					.PrimarySchool()
					.SetFilename(@"60 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1770, 1200)
						.SetSurnameLocation(1835, 1200)
						.SetClassLocation(1900, 1200)
						.SetStudentNoFillingPartLocation(540, 355)
						.SetExamNameLocation(1965, 1200)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1770, 2930)
						.SetSurnameLocation(1835, 2930)
						.SetClassLocation(1900, 2930)
						.SetExamNameLocation(1965, 2930)
						.SetStudentNoFillingPartLocation(540, 2040)
						.Build())
					.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Abcd100)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(50)
					.SetStudentNoYInterval(52)
					.SetStudentNoFillWidth(45)
					.SetDescription("100 - Soruluk Optik Form (ABCD Seçenekli)")
					.PrimarySchool()
					.SetFilename(@"100 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2020, 1200)
						.SetSurnameLocation(2085, 1200)
						.SetClassLocation(2150, 1200)
						.SetExamNameLocation(2215, 1200)
						.SetStudentNoFillingPartLocation(300, 350)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2020, 2930)
						.SetSurnameLocation(2085, 2930)
						.SetClassLocation(2150, 2930)
						.SetExamNameLocation(2215, 2930)
						.SetStudentNoFillingPartLocation(300, 2040)
						.Build())
					.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.ScholarshipPrimary)
						.HasBoxForStudentNumber()
						.SetStudentNoXInterval(50)
						.SetStudentNoYInterval(52)
						.SetStudentNoFillWidth(45)
						.SetDescription("Bursluluk Sınavı Optik Formu (4 Ders-100 Soru)")
						.PrimarySchool()
						.SetFilename(@"Burs.jpg")
						.SetStudentNumberFillDirection(Direction.ToBottom)
						.SetTextDirection(Direction.ToTop)
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(2020, 1200)
							.SetSurnameLocation(2085, 1200)
							.SetClassLocation(2150, 1200)
							.SetExamNameLocation(2215, 1200)
							.SetStudentNoFillingPartLocation(300, 350)
							.Build())
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(2020, 2930)
							.SetSurnameLocation(2085, 2930)
							.SetClassLocation(2150, 2930)
							.SetExamNameLocation(2215, 2930)
							.SetStudentNoFillingPartLocation(300, 2040)
							.Build())
						.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Abc30)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("30 - Soruluk Optik Form (ABC Seçenekli)")
					.PrimarySchool()
					.SetFilename(@"30 Soruluk - ABC.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1600, 1000)
						.SetSurnameLocation(1665, 1000)
						.SetClassLocation(1730, 1000)
						.SetExamNameLocation(1780, 1000)
						.SetStudentNoFillingPartLocation(630, 500)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1600, 2750)
						.SetSurnameLocation(1665, 2750)
						.SetClassLocation(1730, 2750)
						.SetExamNameLocation(1780, 2750)
						.SetStudentNoFillingPartLocation(630, 2270)
						.Build())
					.Build(),

					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Quiz2Grade)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("2. Sınıf Deneme Sınavı Optik Formu")
					.PrimarySchool()
					.SetFilename(@"2. SINIF - DENEME SINAVI.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 1200)
						.SetSurnameLocation(1825, 1200)
						.SetClassLocation(1890, 1200)
						.SetExamNameLocation(1955, 1200)
						.SetStudentNoFillingPartLocation(490, 365)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 2930)
						.SetSurnameLocation(1825, 2930)
						.SetClassLocation(1890, 2930)
						.SetExamNameLocation(1955, 2930)
						.SetStudentNoFillingPartLocation(490, 2100)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Quiz3Grade)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("3. Sınıf Deneme Sınavı Optik Formu")
					.PrimarySchool()
					.SetFilename(@"3. SINIF - DENEME SINAVI.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2010, 1200)
						.SetSurnameLocation(2075, 1200)
						.SetClassLocation(2150, 1200)
						.SetExamNameLocation(2205, 1200)
						.SetStudentNoFillingPartLocation(310, 365)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2010, 2930)
						.SetSurnameLocation(2075, 2930)
						.SetClassLocation(2150, 2930)
						.SetExamNameLocation(2205, 2930)
						.SetStudentNoFillingPartLocation(310, 2100)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Quiz4Grade)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("4. Sınıf Deneme Sınavı Optik Formu")
					.PrimarySchool()
					.SetFilename(@"4. SINIF - DENEME SINAVI.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2010, 1200)
						.SetSurnameLocation(2075, 1200)
						.SetClassLocation(2150, 1200)
						.SetExamNameLocation(2205, 1200)
						.SetStudentNoFillingPartLocation(310, 365)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2010, 2930)
						.SetSurnameLocation(2075, 2930)
						.SetClassLocation(2150, 2930)
						.SetStudentNoLocation(2205, 2930)
						.SetExamNameLocation(2205, 2930)
						.SetStudentNoFillingPartLocation(310, 2100)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Lgs)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(57)
					.SetStudentNoYInterval(58)
					.SetStudentNoFillWidth(45)
					.SetDescription("LGS Deneme Sinavi Optik Formu-(Tek Oturum-6 Ders-90 Soru)")
					.PrimarySchool()
					.SetFilename(@"LGS.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2015, 1200)
						.SetSurnameLocation(2070, 1200)
						.SetClassLocation(2130, 1200)
						.SetExamNameLocation(2190, 1200)
						.SetStudentNoFillingPartLocation(1570, 955)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2015, 2930)
						.SetSurnameLocation(2070, 2930)
						.SetClassLocation(2130, 2930)
						.SetExamNameLocation(2190, 2930)
						.SetStudentNoFillingPartLocation(1570, 2652)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Lgs2Sayisal)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(61)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("LGS - Sayisal Oturumu Optik Formu (2 Ders-40 Soru)")
					.PrimarySchool()
					.SetFilename(@"LGS-SAYISAL.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToRight)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1550, 1180)
						.SetSurnameLocation(1550, 1240)
						.SetClassLocation(1550, 1300)
						.SetExamNameLocation(1550, 1360)
						.SetStudentNoFillingPartLocation(483, 355)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1550, 2865)
						.SetSurnameLocation(1550, 2925)
						.SetClassLocation(1550, 2985)
						.SetExamNameLocation(1550, 3045)
						.SetStudentNoFillingPartLocation(483, 2040)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Lgs2Sozel)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(61)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("LGS - Sozel Oturumu Optik Formu (4 Ders-50 Soru)")
					.PrimarySchool()
					.SetFilename(@"LGS-SOZEL.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToRight)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1550, 1180)
						.SetSurnameLocation(1550, 1240)
						.SetClassLocation(1550, 1300)
						.SetExamNameLocation(1550, 1360)
						.SetStudentNoFillingPartLocation(483, 355)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1550, 2865)
						.SetSurnameLocation(1550, 2925)
						.SetClassLocation(1550, 2985)
						.SetExamNameLocation(1550, 3045)
						.SetStudentNoFillingPartLocation(483, 2040)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.TEOG2)
						.HasBoxForStudentNumber()
						.SetStudentNoXInterval(62)
						.SetStudentNoYInterval(53)
						.SetStudentNoFillWidth(45)
						.SetDescription("5-6-7 ve 8. Sınıf Deneme Sınavı Optik Formu - 2.Oturum (3 DERS)")
						.PrimarySchool()
						.SetFilename(@"TEOG-2.jpg")
						.SetStudentNumberFillDirection(Direction.ToBottom)
						.SetTextDirection(Direction.ToRight)
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(1780, 1200)
							.SetSurnameLocation(1845, 1200)
							.SetClassLocation(1910, 1200)
							.SetExamNameLocation(1965, 1200)
							.SetStudentNoFillingPartLocation(540, 360)
							.Build())
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(1780, 2930)
							.SetSurnameLocation(1845, 2930)
							.SetClassLocation(1910, 2930)
							.SetExamNameLocation(1965, 2930)
							.SetStudentNoFillingPartLocation(540, 2040)
							.Build())
						.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.TEOG1)
						.HasBoxForStudentNumber()
						.SetStudentNoXInterval(62)
						.SetStudentNoYInterval(53)
						.SetStudentNoFillWidth(45)
						.SetDescription("5-6-7 ve 8. Sınıf Deneme Sınavı Optik Formu - 1.Oturum (3 DERS)")
						.PrimarySchool()
						.SetFilename(@"TEOG-1.jpg")
						.SetStudentNumberFillDirection(Direction.ToBottom)
						.SetTextDirection(Direction.ToRight)
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(1780, 1200)
							.SetSurnameLocation(1845, 1200)
							.SetClassLocation(1910, 1200)
							.SetExamNameLocation(1965, 1200)
							.SetStudentNoFillingPartLocation(540, 360)
							.Build())
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(1780, 2930)
							.SetSurnameLocation(1845, 2930)
							.SetClassLocation(1910, 2930)
							.SetExamNameLocation(1965, 2930)
							.SetStudentNoFillingPartLocation(540, 2040)
							.Build())
						.Build(),

					// HighSchool
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.High20)
					.SetStudentNoXInterval(53)
					.SetStudentNoYInterval(62)
					.SetStudentNoFillWidth(45)
					.SetDescription("20 - Soruluk Optik Form")
					.HighSchool()
					.SetFilename("20 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToLeft)
					.SetTextDirection(Direction.ToBottom)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(290, 525)
						.SetSurnameLocation(225, 525)
						.SetClassLocation(160, 525)
						.SetStudentNoLocation(160, 1065)
						.SetStudentNoFillingPartLocation(963, 419)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1480, 525)
						.SetSurnameLocation(1415, 525)
						.SetClassLocation(1350, 525)
						.SetStudentNoLocation(1350, 1065)
						.SetStudentNoFillingPartLocation(2159, 419)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(290, 2290)
						.SetSurnameLocation(225, 2290)
						.SetClassLocation(160, 2290)
						.SetStudentNoLocation(160, 2835)
						.SetStudentNoFillingPartLocation(963, 2128)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1480, 2290)
						.SetSurnameLocation(1415, 2290)
						.SetClassLocation(1350, 2290)
						.SetStudentNoLocation(1350, 2835)
						.SetStudentNoFillingPartLocation(2159, 2128)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.High30)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("30 - Soruluk Optik Form")
					.HighSchool()
					.SetFilename("30 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1650, 1000)
						.SetSurnameLocation(1715, 1000)
						.SetClassLocation(1780, 1000)
						.SetExamNameLocation(1845, 1000)
						.SetStudentNoFillingPartLocation(580, 500)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1650, 2780)
						.SetSurnameLocation(1715, 2780)
						.SetClassLocation(1780, 2780)
						.SetExamNameLocation(1845, 2780)
						.SetStudentNoFillingPartLocation(580, 2270)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.High60)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("60 - Soruluk Optik Form")
					.HighSchool()
					.SetFilename("60 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1870, 1200)
						.SetSurnameLocation(1935, 1200)
						.SetClassLocation(2000, 1200)
						.SetExamNameLocation(2065, 1200)
						.SetStudentNoFillingPartLocation(440, 355)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1870, 2930)
						.SetSurnameLocation(1935, 2930)
						.SetClassLocation(2000, 2930)
						.SetExamNameLocation(2065, 2930)
						.SetStudentNoFillingPartLocation(440, 2040)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.High100)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(50)
					.SetStudentNoYInterval(52)
					.SetStudentNoFillWidth(45)
					.SetDescription("100 - Soruluk Optik Form")
					.HighSchool()
					.SetFilename("100 Soruluk.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2020, 1200)
						.SetSurnameLocation(2085, 1200)
						.SetClassLocation(2150, 1200)
						.SetExamNameLocation(2215, 1200)
						.SetStudentNoFillingPartLocation(300, 350)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2020, 2930)
						.SetSurnameLocation(2085, 2930)
						.SetClassLocation(2150, 2930)
						.SetExamNameLocation(2215, 2930)
						.SetStudentNoFillingPartLocation(300, 2040)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Tyt1)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("TYT-1(Turkce - Sos. Bil.) Optik Formu")
					.HighSchool()
					.SetFilename("TYT - 1.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 1200)
						.SetSurnameLocation(1825, 1200)
						.SetClassLocation(1890, 1200)
						.SetExamNameLocation(1925, 1200)
						.SetStudentNoFillingPartLocation(400, 359)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 2930)
						.SetSurnameLocation(1825, 2930)
						.SetClassLocation(1890, 2930)
						.SetExamNameLocation(1925, 2930)
						.SetStudentNoFillingPartLocation(400, 2046)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Tyt2)
					.HasBoxForStudentNumber()
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("TYT-2(T.Mat - Fen Bil.) Optik Formu")
					.HighSchool()
					.SetFilename("TYT - 2.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 1200)
						.SetSurnameLocation(1825, 1200)
						.SetClassLocation(1890, 1200)
						.SetExamNameLocation(1925, 1200)
						.SetStudentNoFillingPartLocation(400, 359)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(1760, 2930)
						.SetSurnameLocation(1825, 2930)
						.SetClassLocation(1890, 2930)
						.SetExamNameLocation(1925, 2930)
						.SetStudentNoFillingPartLocation(400, 2046)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Ayt1)
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("AYT-1 Optik Formu")
					.HighSchool()
					.HasBoxForStudentNumber()
					.SetFilename("AYT-1.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 1200)
						.SetSurnameLocation(2165, 1200)
						.SetClassLocation(2230, 1200)
						.SetExamNameLocation(2280, 1200)
						.SetStudentNoFillingPartLocation(248, 356)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 2930)
						.SetSurnameLocation(2165, 2930)
						.SetClassLocation(2230, 2930)
						.SetExamNameLocation(2280, 2930)
						.SetStudentNoFillingPartLocation(248, 2047)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Ayt2)
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("AYT-2 Optik Formu")
					.HighSchool()
					.HasBoxForStudentNumber()
					.SetFilename("AYT-2.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 1200)
						.SetSurnameLocation(2165, 1200)
						.SetClassLocation(2230, 1200)
						.SetExamNameLocation(2280, 1200)
						.SetStudentNoFillingPartLocation(248, 356)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 2930)
						.SetSurnameLocation(2165, 2930)
						.SetClassLocation(2230, 2930)
						.SetExamNameLocation(2280, 2930)
						.SetStudentNoFillingPartLocation(248, 2047)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.Aytlang)
					.SetStudentNoXInterval(62)
					.SetStudentNoYInterval(53)
					.SetStudentNoFillWidth(45)
					.SetDescription("AYT Dil Optik Formu")
					.HighSchool()
					.HasBoxForStudentNumber()
					.SetFilename("ayt-dil.jpg")
					.SetStudentNumberFillDirection(Direction.ToBottom)
					.SetTextDirection(Direction.ToTop)
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 1200)
						.SetSurnameLocation(2165, 1200)
						.SetClassLocation(2230, 1200)
						.SetExamNameLocation(2280, 1200)
						.SetStudentNoFillingPartLocation(248, 356)
						.Build())
					.AddTextLocation(new OpticalFormTextLocationBuilder()
						.SetNameLocation(2100, 2930)
						.SetSurnameLocation(2165, 2930)
						.SetClassLocation(2230, 2930)
						.SetExamNameLocation(2280, 2930)
						.SetStudentNoFillingPartLocation(248, 2047)
						.Build())
					.Build(),
					new OpticalFormDefinitionBuilder(OpticalFormDefinitions.ScholarshipHigh)
						.HasBoxForStudentNumber()
						.SetStudentNoXInterval(50)
						.SetStudentNoYInterval(52)
						.SetStudentNoFillWidth(45)
						.SetDescription("Bursluluk Sınavı Optik Formu (4 Ders-100 Soru)")
						.HighSchool()
						.SetFilename("Burs-Lise.jpg")
						.SetStudentNumberFillDirection(Direction.ToBottom)
						.SetTextDirection(Direction.ToTop)
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(2020, 1200)
							.SetSurnameLocation(2085, 1200)
							.SetClassLocation(2150, 1200)
							.SetExamNameLocation(2215, 1200)
							.SetStudentNoFillingPartLocation(300, 350)
							.Build())
						.AddTextLocation(new OpticalFormTextLocationBuilder()
							.SetNameLocation(2020, 2930)
							.SetSurnameLocation(2085, 2930)
							.SetClassLocation(2150, 2930)
							.SetExamNameLocation(2215, 2930)
							.SetStudentNoFillingPartLocation(300, 2040)
							.Build())
						.Build(),
				};
			context.OpticalFormDefinitions.AddRange(formDefinitions);
			await context.SaveChangesAsync();
			var formTypes = new List<OpticalFormType>
			{
				new OpticalFormType(
					OpticalFormTypes.Names.Frm100ABCD,
					OpticalFormTypes.Codes.Frm100ABCD,
					"ilk-orta-4sinif.yap",
					SchoolType.PrimaryAndSecondary,
					null,
					100),
			};

			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Abcd100));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm3rdGradeTrial,
				OpticalFormTypes.Codes.Frm3rdGradeTrial,
				"ilk-orta-3.sinif.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20),
					new FormLessonSection(matLesson, 20),
					new FormLessonSection(scienceLesson, 20),
					new FormLessonSection(hbLesson, 20),
					new FormLessonSection(langLesson, 20),
				},
				80));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Quiz3Grade));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm4thGradeTrial,
				OpticalFormTypes.Codes.Frm4thGradeTrial,
				"ilk-orta-4sinif.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20),
					new FormLessonSection(matLesson, 20),
					new FormLessonSection(scienceLesson, 20),
					new FormLessonSection(socLesson, 20),
					new FormLessonSection(langLesson, 20),
				},
				100));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Quiz4Grade));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm100ABCDE,
				OpticalFormTypes.Codes.Frm100ABCDE,
				"Lise-100luk.yap",
				SchoolType.High,
				null,
				100));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.High100));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm20ABCD,
				OpticalFormTypes.Codes.Frm20ABCD,
				"ilk-orta-20lik.yap",
				SchoolType.PrimaryAndSecondary,
				null,
				20));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Abcd20));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm20ABCDE,
				OpticalFormTypes.Codes.Frm20ABCDE,
				"Lise-20lik.yap",
				SchoolType.High,
				null,
				20));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.High20));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm30ABCDE,
				OpticalFormTypes.Codes.Frm30ABCDE,
				"Lise-30luk.yap",
				SchoolType.High,
				null,
				30));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.High30));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm30ABC,
				OpticalFormTypes.Codes.Frm30ABC,
				"ilk-orta-30luk-ABC.yap",
				SchoolType.PrimaryAndSecondary,
				null,
				30));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Abc30));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm30ABCD,
				OpticalFormTypes.Codes.Frm30ABCD,
				"ilk-orta-30luk.yap",
				SchoolType.PrimaryAndSecondary,
				null,
				30));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Abcd30));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm60ABCD,
				OpticalFormTypes.Codes.Frm60ABCD,
				"ilk-orta-60lik.yap",
				SchoolType.PrimaryAndSecondary,
				null,
				60));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Abcd60));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm60ABCDE,
				OpticalFormTypes.Codes.Frm60ABCDE,
				"Lise-60lik.yap",
				SchoolType.High,
				null,
				60));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.High60));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.Frm2ndGradeTrial,
				OpticalFormTypes.Codes.Frm2ndGradeTrial,
				"ilk-orta-2.sinif.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20),
					new FormLessonSection(matLesson, 20),
					new FormLessonSection(hbLesson, 20),
					new FormLessonSection(langLesson, 20),
				},
				80));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Quiz2Grade));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmLgs,
				OpticalFormTypes.Codes.FrmLgs,
				"LGS.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20),
					new FormLessonSection(matLesson, 20),
					new FormLessonSection(scienceLesson, 20),
					new FormLessonSection(socLesson, 10),
					new FormLessonSection(relLesson, 10),
					new FormLessonSection(langLesson, 10),
				},
				90));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Lgs));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmLgsTwoPiece,
				OpticalFormTypes.Codes.FrmLgsTwoPiece,
				"LGS2.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20, 1),
					new FormLessonSection(matLesson, 20, 2),
					new FormLessonSection(scienceLesson, 20, 2),
					new FormLessonSection(socLesson, 10, 1),
					new FormLessonSection(relLesson, 10, 1),
					new FormLessonSection(langLesson, 10, 1),
				},
				120));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Lgs2Sayisal));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Lgs2Sozel));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmScholarship,
				OpticalFormTypes.Codes.FrmScholarship,
				"ilk-orta-100luk.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 25),
					new FormLessonSection(matLesson, 25),
					new FormLessonSection(scienceLesson, 25),
					new FormLessonSection(socLesson, 25),
				},
				100));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.ScholarshipPrimary));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmScholarshipHigh,
				OpticalFormTypes.Codes.FrmScholarshipHigh,
				@"Lise-100luk.yap",
				SchoolType.High,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 25),
					new FormLessonSection(matLesson, 25),
					new FormLessonSection(scienceLesson, 25),
					new FormLessonSection(socLesson, 25),
				},
				100));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.ScholarshipHigh));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmTeog,
				OpticalFormTypes.Codes.FrmTeog,
				"TEOG.yap",
				SchoolType.PrimaryAndSecondary,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 20, 1),
					new FormLessonSection(matLesson, 20, 1),
					new FormLessonSection(scienceLesson, 20, 2),
					new FormLessonSection(socLesson, 20, 2),
					new FormLessonSection(relLesson, 20, 1),
					new FormLessonSection(langLesson, 20, 2),
				},
				120));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.TEOG1));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.TEOG2));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmTyt,
				OpticalFormTypes.Codes.FrmTyt,
				"Lise-TYT.yap",
				SchoolType.High,
				new List<FormLessonSection>
				{
					new FormLessonSection(trLesson, 40, 1),
					new FormLessonSection(socLesson, 20, 1),
					new FormLessonSection(matLesson, 40, FormTypeLessonTags.TytMathematics, 2),
					new FormLessonSection(scienceLesson, 20, 2),
				},
				120));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Tyt1));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Tyt2));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmAyt,
				OpticalFormTypes.Codes.FrmAyt,
				"Lise-YKS.yap",
				SchoolType.High,
				new List<FormLessonSection>
				{
					new FormLessonSection(litLesson, 40, FormTypeLessonTags.AytLiterature, 1),
					new FormLessonSection(socLesson, 40, FormTypeLessonTags.AytSocialScience, 1),
					new FormLessonSection(matLesson, 40, 2),
					new FormLessonSection(scienceLesson, 40, 2),
				},
				160));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Ayt1));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Ayt2));

			formTypes.Add(new OpticalFormType(
				OpticalFormTypes.Names.FrmAytLang,
				OpticalFormTypes.Codes.FrmAytLang,
				"Lise-Dil.yap",
				SchoolType.High,
				new List<FormLessonSection>
				{
					new FormLessonSection(langLesson, 80),
				},
				80));
			formTypes.Last().AddOpticalFormDefinition(
				formDefinitions.First(f => f.Name == OpticalFormDefinitions.Aytlang));

			context.FormTypes.AddRange(formTypes);

			foreach (var formType in context.FormTypes.Local)
			{
				var sections = formType.FormLessonSections.ToList();

				foreach (var section in formType.FormLessonSections)
				{
					context.Entry(section).Property("ListOrder").CurrentValue =
						sections.IndexOf(section) + 1;
				}
			}

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