namespace TestOkur.WebApi.Application.Score
{
    using FluentValidation;
    using TestOkur.Common;

    public class EditScoreFormulaCommandValidator : AbstractValidator<EditScoreFormulaCommand>
	{
		public EditScoreFormulaCommandValidator()
		{
			RuleFor(m => m.BasePoint)
				.GreaterThan(0)
				.WithMessage(ErrorCodes.BasePointMustBeGreaterThanZero);

			RuleFor(m => m.ScoreFormulaId)
				.Id(ErrorCodes.InvalidScoreFormulaId);
		}
	}
}
