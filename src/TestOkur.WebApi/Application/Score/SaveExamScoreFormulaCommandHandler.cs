namespace TestOkur.WebApi.Application.Score
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Paramore.Brighter;
	using TestOkur.Data;
	using TestOkur.Domain.Model.ScoreModel;
	using TestOkur.Infrastructure.Cqrs;
	using Exam = TestOkur.Domain.Model.ExamModel.Exam;

	public sealed class SaveExamScoreFormulaCommandHandler
		: RequestHandlerAsync<SaveExamScoreFormulaCommand>
	{
		private readonly ApplicationDbContext _dbContext;

		public SaveExamScoreFormulaCommandHandler(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		[Idempotent(1)]
		[Populate(2)]
		[ClearCache(3)]
		public override async Task<SaveExamScoreFormulaCommand> HandleAsync(
			SaveExamScoreFormulaCommand command,
			CancellationToken cancellationToken = default)
		{
			var examScoreFormula = await GetExamScoreFormulaAsync(
				command.ExamId,
				command.UserId,
				cancellationToken);

			if (examScoreFormula != null)
			{
				_dbContext.Remove(examScoreFormula);
			}

			var exam = await GetExamAsync(
				command.ExamId,
				command.UserId,
				cancellationToken);
			var formula = await GetScoreFormulaAsync(
				command.OriginalFormulaId,
				command.UserId,
				cancellationToken);
			var coefficients = formula.Coefficients
				.Select(fcoef =>
					new LessonCoefficient(fcoef.ExamLessonSection, command.Coefficients[(int)fcoef.Id]))
				.ToList();

			examScoreFormula = new ExamScoreFormula(
				exam,
				formula.Name.Value,
				formula.Grade.Value,
				command.BasePoint,
				formula.FormulaType,
				coefficients);
			_dbContext.ExamScoreFormulas.Add(examScoreFormula);
			await _dbContext.SaveChangesAsync(cancellationToken);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task<ExamScoreFormula> GetExamScoreFormulaAsync(
			int examId,
			int userId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.ExamScoreFormulas
				.FirstOrDefaultAsync(
					e => e.Exam.Id == examId &&
					     EF.Property<int>(e, "CreatedBy") == userId,
					cancellationToken);
		}

		private async Task<Exam> GetExamAsync(int examId, int userId, CancellationToken cancellationToken)
		{
			return await _dbContext.Exams
				.Include(e => e.ExamType)
				.FirstAsync(
					e => e.Id == examId &&
					     EF.Property<int>(e, "CreatedBy") == userId,
					cancellationToken);
		}

		private async Task<ScoreFormula> GetScoreFormulaAsync(
			int id,
			int userId,
			CancellationToken cancellationToken)
		{
			return await _dbContext.ScoreFormulas
				.Include(s => s.FormulaType)
				.Include(s => s.Coefficients)
				.ThenInclude(c => c.ExamLessonSection)
				.ThenInclude(e => e.Lesson)
				.FirstAsync(
					s => EF.Property<int>(s, "CreatedBy") == userId && s.Id == id,
					cancellationToken);
		}
	}
}
