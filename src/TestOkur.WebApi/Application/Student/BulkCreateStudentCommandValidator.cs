namespace TestOkur.WebApi.Application.Student
{
    using FluentValidation;

    public class BulkCreateStudentCommandValidator : AbstractValidator<BulkCreateStudentCommand>
    {
        public BulkCreateStudentCommandValidator()
        {
            RuleFor(m => m.Commands)
                .NotEmpty();

            var validator = new CreateStudentCommandValidator();
            RuleForEach(m => m.Commands)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(validator);
        }
    }
}
