namespace TestOkur.WebApi.Data
{
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TestOkur.Data;
	using TestOkur.Domain.Model.ExamModel;
	using TestOkur.Domain.Model.OpticalFormModel;

	internal class ExamTypeSeeder : ISeeder
	{
		private List<OpticalFormType> _formTypes;

		private ExamType EvaluationExam => new ExamTypeBuilder(ExamTypes.EvaluationExam)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABC))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCDE))
					.WithOrder(20)
					.Build();

		private ExamType LgsExam => new ExamTypeBuilder(ExamTypes.Lgs)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmLgs))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmLgsTwoPiece))
					.WithOrder(40)
					.WithDefaultIncorrectCorrectEliminationValue(3)
					.Build();

		private ExamType TytExam => new ExamTypeBuilder(ExamTypes.Tyt)
					.WithDefaultIncorrectCorrectEliminationValue(4)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmTyt))
					.WithOrder(50)
					.Build();

		private ExamType AytLangExam => new ExamTypeBuilder(ExamTypes.AytLang)
					.WithDefaultIncorrectCorrectEliminationValue(4)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmAytLang))
					.WithOrder(70)
					.Build();

		private ExamType AytExam => new ExamTypeBuilder(ExamTypes.Ayt)
					.WithDefaultIncorrectCorrectEliminationValue(4)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmAyt))
					.WithOrder(60)
					.Build();

		private ExamType TrialExam => new ExamTypeBuilder(ExamTypes.TrialExam)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm2ndGradeTrial))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm3rdGradeTrial))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm4thGradeTrial))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmTeog))
					.WithOrder(30)
					.Build();

		private ExamType ScholarshipExam => new ExamTypeBuilder(ExamTypes.Scholarship)
					.WithDefaultIncorrectCorrectEliminationValue(3)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmScholarship))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.FrmScholarshipHigh))
					.WithOrder(50)
					.Build();

		private ExamType SingleLessonExam => new ExamTypeBuilder(ExamTypes.LessonExam)
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABC))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCD))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm30ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm20ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm60ABCDE))
					.AddFormType(_formTypes.First(f => f.Code == OpticalFormTypes.Codes.Frm100ABCDE))
					.WithOrder(10)
					.Build();

		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider services)
		{
			if (await dbContext.ExamTypes.AnyAsync())
			{
				return;
			}

			_formTypes = await dbContext.FormTypes.ToListAsync();
			dbContext.Add(SingleLessonExam);
			dbContext.Add(EvaluationExam);
			dbContext.Add(LgsExam);
			dbContext.Add(TytExam);
			dbContext.Add(AytExam);
			dbContext.Add(AytLangExam);
			dbContext.Add(TrialExam);
			dbContext.Add(ScholarshipExam);

			await dbContext.SaveChangesAsync();
		}
	}
}
