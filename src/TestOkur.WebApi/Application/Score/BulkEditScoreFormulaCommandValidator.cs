namespace TestOkur.WebApi.Application.Score
{
    using FluentValidation;

    public class BulkEditScoreFormulaCommandValidator : AbstractValidator<BulkEditScoreFormulaCommand>
	{
		public BulkEditScoreFormulaCommandValidator()
		{
			RuleFor(m => m.Commands)
				.NotEmpty();

			var validator = new EditScoreFormulaCommandValidator();
			RuleForEach(m => m.Commands)
				.Cascade(CascadeMode.StopOnFirstFailure)
				.SetValidator(validator);
		}
	}
}
